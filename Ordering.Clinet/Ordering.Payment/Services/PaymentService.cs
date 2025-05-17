using Grpc.Core;
using Ordering.Payment.Data;
using Ordering.Payment.Protos;

namespace Ordering.Payment.Services
{
    public class PaymentService : UserPaymentService.UserPaymentServiceBase
    {
        public override Task<UserPaymentResponse> ProcessUserPayment(UserPaymentRequest request, ServerCallContext context)
        {
            var database = new Database();
            var selectedUser = database.GetUsers().Where(usr => request.UserID == usr.Id).FirstOrDefault();

            PaymentResponseStatus responseStatus = PaymentResponseStatus.Success;

            if (selectedUser == null)
                responseStatus = PaymentResponseStatus.UserNotFound;
            
            else if (request.UserBalance > selectedUser.Balance) 
                responseStatus = PaymentResponseStatus.Failed;
            

            return Task.FromResult(new UserPaymentResponse
            {
                UserID = request.UserID,
                PaymentStatus = responseStatus,
                RemainingUserBalance = responseStatus == PaymentResponseStatus.UserNotFound 
                ? 0 
                : selectedUser.Balance - request.UserBalance
            });

        }
    }
}
