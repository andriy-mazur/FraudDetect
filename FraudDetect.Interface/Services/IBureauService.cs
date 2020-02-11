namespace FraudDetect.Interface.Services
{
    using System.Threading.Tasks;
    using FraudDetect.Interface.Model;

    public interface IBureauService
    {
        Task<Response> GetScoreAsync(Request request);
    }
}
