namespace Wasalnyy.BLL.Service.Abstraction
{
	public interface IFaceService
	{
		double[] ExtractEmbedding(byte[] imageBytes);
		double CompareEmbeddings(double[] emb1, double[] emb2);
		Task<AuthResult> RegisterDriverFaceAsync(string driverId, byte[] faceImage);
		Task<AuthResult> FaceLoginAsync(byte[] faceImage);
	}
}
