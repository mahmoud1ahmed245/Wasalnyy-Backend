using FaceRecognitionDotNet;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using Wasalnyy.BLL.JwtHandling;
using Wasalnyy.BLL.Service.Abstraction;

namespace Wasalnyy.BLL.Service.Implementation
{
	public class FaceService : IFaceService
	{
		private readonly FaceRecognition _faceRecognition;
		private readonly IUserFaceDataRepo _faceRepo;
		private readonly UserManager<User> _userManager;
		private readonly JwtHandler _jwtHandler;
		private readonly SignInManager<User> _signInManager;
		public FaceService(
			FaceRecognition faceRecognition, 
			IUserFaceDataRepo faceRepo, 
			UserManager<User> userManager, 
			SignInManager<User> signInManager, 
			JwtHandler jwtHandler)
		{
			_faceRecognition = faceRecognition;
			_faceRepo = faceRepo;
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtHandler = jwtHandler;
		}

		public double[] ExtractEmbedding(byte[] imageBytes)
		{
			using var ms = new MemoryStream(imageBytes);
			using var bmp = new Bitmap(ms);
			using var img = FaceRecognition.LoadImage(bmp);

			var encodings = _faceRecognition.FaceEncodings(img).ToList();
			if (!encodings.Any())
				throw new Exception("No face detected");

			return encodings[0].GetRawEncoding();
		}

		public double CompareEmbeddings(double[] emb1, double[] emb2)
		{
			double sum = 0;
			for (int i = 0; i < emb1.Length; i++)
				sum += Math.Pow(emb1[i] - emb2[i], 2);
			return Math.Sqrt(sum);
		}
		public async Task<AuthResult> RegisterDriverFaceAsync(string driverId, byte[] faceImage)
		{
			var driver = await _userManager.FindByIdAsync(driverId);
			if (driver == null)
				return new AuthResult { Success = false, Message = "Driver not found" };
			double[] embedding;
			try
			{
				embedding = ExtractEmbedding(faceImage);
			}
			catch
			{
				return new AuthResult { Success = false, Message = "No face detected" };
			}
			var faceData = new UserFaceData
			{
				DriverId = driver.Id,
				Embedding = EmbeddingSerializer.DoubleArrayToBytes(embedding)
			};
			await _faceRepo.AddAsync(faceData);
			return new AuthResult { Success = true, Message = "Face registered successfully" };
		}
		public async Task<AuthResult> FaceLoginAsync(byte[] faceImage)
		{
			double[] incomingEmbedding;
			try
			{
				incomingEmbedding = ExtractEmbedding(faceImage);
			}
			catch
			{
				return new AuthResult { Success = false, Message = "No face detected in image" };
			}

			var drivers = await _faceRepo.GetAllDriversAsync();
			User? matchedUser = null;
			double bestDistance = double.MaxValue;
			foreach (var d in drivers)
			{
				double[] storedEmbedding = EmbeddingSerializer.BytesToDoubleArray(d.Embedding);
				double distance = CompareEmbeddings(storedEmbedding, incomingEmbedding);
				if (distance < bestDistance)
				{
					bestDistance = distance;
					matchedUser = d.Driver;
				}
			}
			const double THRESHOLD = 0.6; // tune this
			if (matchedUser != null && bestDistance <= THRESHOLD)
			{
				await _signInManager.SignInAsync(matchedUser, isPersistent: false);
				var token = await _jwtHandler.GenerateToken(matchedUser);
				return new AuthResult { Success = true, Message = "Face login successful", Token = token };
			}
			return new AuthResult { Success = false, Message = "Face not recognized" };
		}
	}
}
