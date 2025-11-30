namespace Wasalnyy.DAL.Configuration
{
    internal class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            //  Only prevent same person reviewing twice on same trip
            builder.HasIndex(r => new { r.TripId, r.ReviewerType })
                .IsUnique()
                .HasName("IX_Reviews_UniquePerTrip");

            builder.HasOne(r => r.Rider)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.RiderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Driver)
                .WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Trip)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TripId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}