using GymSystem.Dtos.Payment;

namespace GymSystem.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto>
            CreatePaymentAsync(CreatePaymentDto dto);

        Task<PaymentResponseDto>
            GetByIdAsync(int id);

        Task<PagedPaymentResponseDto>
            GetAllAsync(PaymentQueryDto query);
    }
}
