namespace FraudDetect.Interface
{
    using System.Threading.Tasks;
    using FraudDetect.Interface.Model;

    public interface IBureauService
    {
        Task<Response> GetScoreAsync(Request request);
    }
}
