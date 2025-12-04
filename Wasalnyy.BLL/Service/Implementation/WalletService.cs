
using Microsoft.AspNetCore.Identity;
namespace Wasalnyy.BLL.Service.Implementation
{
	public class WalletService : IWalletService
    {
        private readonly IWalletRepo _walletRepo;
        private readonly IWalletTransactionLogsRepo _transactionRepo;
        private readonly WasalnyyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITripRepo tripRepo;
        private readonly IWalletMoneyTransfersService WalletMoneyTransfersService;
        private readonly RiderService riderService;
        private readonly DriverService driverService;
        private readonly IWalletTransactionService walletTransactionService;
		private readonly UserManager<User> _userManager;
		public WalletService(
			IWalletRepo walletRepo,
			IWalletTransactionLogsRepo transactionRepo,
			WasalnyyDbContext context,
			IMapper mapper,
			ITripRepo tripRepo,
			IWalletTransactionService walletTransactionService,
			IWalletMoneyTransfersService WalletMoneyTransfersService,
			RiderService riderService,
			DriverService driverService,
			UserManager<User> userManager)
		{
			_walletRepo = walletRepo;
			_transactionRepo = transactionRepo;
			_context = context;
			_mapper = mapper;
			this.tripRepo = tripRepo;
			this.walletTransactionService = walletTransactionService;
			this.WalletMoneyTransfersService = WalletMoneyTransfersService;
			this.riderService = riderService;
			this.driverService = driverService;
			_userManager = userManager;
		}
		public async Task<Wallet?> GetWalletOfUserIdAsync(string userId)
        {
            return await _walletRepo.GetWalletOfUserIdAsync(userId);
        }
        public async Task<IncreaseWalletBalanceResponse> IncreaseWalletAsync(IncreaseWalletDTO increaseWalletDTO)
        {
            if (increaseWalletDTO.Amount <= 0)
                return new IncreaseWalletBalanceResponse(false ,"Amount of money cant be negative or zero");
            try
            {
                var wallet = await _walletRepo.GetWalletOfUserIdAsync(increaseWalletDTO.UserId);
                if (wallet == null)
                    return new IncreaseWalletBalanceResponse(false, "This User doesnt have Wallet Call Dev to make sure Rider or driver User have wallet created");
                
                wallet.Balance += increaseWalletDTO.Amount;
                wallet.ModifiedAt = increaseWalletDTO.DateTime;
                await _walletRepo.UpdateWalletAsync(wallet);
                await _walletRepo.SaveChangesAsync();

                
                var res=  await walletTransactionService.CreateAsync(new CreateWalletTransactionLogDTO
                {
                    WalletId = wallet.Id,
                    Amount = increaseWalletDTO.Amount,
                    TransactionType = DAL.Enum.WalletTransactionType.Credit,
                    Description = $"user charge his wallet by {increaseWalletDTO.Amount}",
                    CreatedAt = increaseWalletDTO.DateTime

                });
                if(!res.isSuccess)
                    return new IncreaseWalletBalanceResponse(false, $"balance value is increased but An error occurred while creating wallet transaction log: {res.Message}");

                return new IncreaseWalletBalanceResponse(true, "Wallet balance increased successfully");
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new IncreaseWalletBalanceResponse(false, $"An error occurred while processing payment: {innerMessage}");
            }  
        }
        public async Task<TransferWalletResponse> HandleTransferWalletMoneyFromRiderToDriver(TransferMoneyBetweenUsersDTO transferDto)
        {
            //DriverRepo driverRepo = new DriverRepo(_context);
            //ReturnDriverDto driver2 = await driverService.GetByIdAsync("f3aabcbe-a853-4cb9-8409-c010f560f2dc");
            using var transaction =  await _walletRepo.BeginTransactionAsync();
            try
            {
                //1-check if this rider valid or not  
                var rider = await riderService.GetByIdAsync(transferDto.RiderId);
                if (rider == null)
                    return new TransferWalletResponse(false, "Rider not found ");
                //2-check if this driver valid or not
                var driver = await driverService.GetByIdAsync(transferDto.DriverId);
                if (driver == null)
                    return new TransferWalletResponse(false, "Driver not found ");
                // 3- Get wallets
                var riderWallet = await _walletRepo.GetWalletOfUserIdAsync(transferDto.RiderId);
                var driverWallet = await _walletRepo.GetWalletOfUserIdAsync(transferDto.DriverId);
          
                if ( driverWallet == null)
                    return new TransferWalletResponse(false, "Driver Wallet not found ");
                // 4- Check balance
                if (riderWallet.Balance < transferDto.Amount)
                    return new TransferWalletResponse(false, "Insufficient balance");
                //3-check if the trip id is exist or not 
                var trip=  await tripRepo.GetByIdAsync(transferDto.TripId);
                if (trip == null)
                    return new TransferWalletResponse(false, "Transfering failed Trip not found ");
                // 5- update balances and update Lasttimeupdatedate date 
                riderWallet.Balance -= transferDto.Amount;
                driverWallet.Balance += transferDto.Amount;
                riderWallet.ModifiedAt = transferDto.CreatedAt;
                driverWallet.ModifiedAt = transferDto.CreatedAt;

                await   _walletRepo.UpdateWalletWithoutSaving(riderWallet);
                await   _walletRepo.UpdateWalletWithoutSaving(driverWallet);
                //6- create  WALLET transaction log for both wallets
                //Rider wallet TransactionLog
                var res = await walletTransactionService.CreateAsync(new CreateWalletTransactionLogDTO
                {
                    WalletId = riderWallet.Id,
                    Amount = transferDto.Amount,
                    TransactionType = DAL.Enum.WalletTransactionType.Debit,
                    Description = $"Rider Pay {transferDto.Amount} for trip ID {trip.Id} to Driver Id{driver.Id}",
                    CreatedAt = transferDto.CreatedAt
                });
                if (!res.isSuccess)
                {
                    await transaction.RollbackAsync();
                    return new TransferWalletResponse(false, $"Failed to create rider transaction log: {res.Message}");
                }

                var res2 = await walletTransactionService.CreateAsync(new CreateWalletTransactionLogDTO
                {
                    WalletId = driverWallet.Id,
                    Amount = transferDto.Amount,
                    TransactionType = DAL.Enum.WalletTransactionType.Credit,
                    Description = $"Driver receive {transferDto.Amount} for trip ID {trip.Id} from Rider Id{rider.RiderId}",
                    CreatedAt = transferDto.CreatedAt
                });
                if (!res2.isSuccess)
                {
                    await transaction.RollbackAsync();
                    return new TransferWalletResponse(false, $"Transfering failed An error occurred while creating wallet transaction log: {res2.Message}");
                }
                //7-insert this transfer transaction in the transferTransaction table
                var res3=  await WalletMoneyTransfersService.AddAsync(new AddWalletTranferMoneyDTO
                {
                    CreatedAt = transferDto.CreatedAt,
                    Amount = transferDto.Amount,
                    SenderWalletId = riderWallet.Id,
                    ReceiverWalletId = driverWallet.Id,
                    TripId = transferDto.TripId,
                });
                if (!res3.IsSuccess)
                {
                    await transaction.RollbackAsync();
                    return new TransferWalletResponse(false, $"Transfering failed An error occurred while inserting Transfer Transaction  {res2.Message}");
                }
                // 8- save all changes
                await _walletRepo.SaveChangesAsync();
                // 9- commit
                await transaction.CommitAsync();
                return new TransferWalletResponse(true, "Transfer completed successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return new TransferWalletResponse(false, $"Transaction failed: {innerMessage}");
            }
        }
        public async Task<CreateWalletResponse> CreateWalletAsync(CreateWalletDTO createWalletDTO)
        {
            // Map DTO to entity
            var wallet = _mapper.Map<Wallet>(createWalletDTO);
            // Save to repository
            try
            {
                await _walletRepo.CreateAsync(wallet);
                await _walletRepo.SaveChangesAsync();
                return new CreateWalletResponse(true, "Wallet Created succesfully ");
            }
            catch  (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CreateWalletResponse(false, $"Creating Wallet Failed: {innerMessage}");
            }
        }
        public async Task<WithDrawFromWalletResponse> WithdrawFromWalletAsync(WithdrawFromWalletDto withdrawFromWalletDto)
        {
            //var TransactionLog = _mapper.Map<CreateWalletTransactionDTO>(withdrawFromWalletDto);
            if (withdrawFromWalletDto.Amount <= 0)
                return new WithDrawFromWalletResponse(false,"Amount Can't be negative or zero");

            var wallet = await _walletRepo.GetWalletOfUserIdAsync(withdrawFromWalletDto.UserId);
            if (wallet == null)
                return new WithDrawFromWalletResponse(false, "This User doesnt hvae a wallet");

            if (wallet.Balance < withdrawFromWalletDto.Amount)
               return new WithDrawFromWalletResponse(false, "Balance is not enough");

            wallet.Balance -= withdrawFromWalletDto.Amount;
            wallet.ModifiedAt =withdrawFromWalletDto.CreatedAt;

            await _walletRepo.UpdateWalletAsync(wallet);
            var res = await walletTransactionService.CreateAsync(new CreateWalletTransactionLogDTO
            {
                WalletId = wallet.Id,
                Amount = withdrawFromWalletDto.Amount,
                TransactionType = DAL.Enum.WalletTransactionType.Debit,
                Description = $"user withdraw from his wallet by {withdrawFromWalletDto.Amount}",
                CreatedAt = withdrawFromWalletDto.CreatedAt

            });
            if (!res.isSuccess)
                return new WithDrawFromWalletResponse(false, $"Withdraw done but An error occurred while creating wallet transaction log: {res.Message}");
           return new WithDrawFromWalletResponse(true, "Withdraw from wallet done successfully");
        }
		public async Task CreateWalletForUserAsync(User user)
		{
			// prevent admin from getting wallet
			if (await _userManager.IsInRoleAsync(user, "Admin"))
				return;
			// Check if wallet already exists
			var existingWallet = await _walletRepo.GetWalletOfUserIdAsync(user.Id);
			if (existingWallet != null)
				return;
			var wallet = new Wallet
			{
				UserId = user.Id,
				Balance = 0,
			};
			await _walletRepo.CreateAsync(wallet);
			await _walletRepo.SaveChangesAsync();
		}
		public async Task<bool> CheckUserBalanceAsync(string userId, decimal amount)
        {
            var wallet = await _walletRepo.GetWalletOfUserIdAsync(userId);
            return wallet != null && wallet.Balance >= amount;
        }

		public async Task<decimal> GetWalletBalance(string userId)
		{
			var wallet = await _walletRepo.GetWalletOfUserIdAsync(userId);
			if (wallet == null)
				throw new InvalidOperationException("Wallet not found for the specified user ID.");
			return wallet.Balance;
		}
	}
}

