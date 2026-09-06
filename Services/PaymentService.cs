using GymSystem.Dtos.Payment;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMemberMembershipRepository
            _memberMembershipRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IMemberMembershipRepository memberMembershipRepository)
        {
            _paymentRepository = paymentRepository;
            _memberMembershipRepository =
                memberMembershipRepository;
        }

        public async Task<PaymentResponseDto>
            CreatePaymentAsync(CreatePaymentDto dto)
        {
            // Check membership exists
            var membership =
                await _memberMembershipRepository
                    .GetByIdAsync(dto.MemberMembershipId);

            if (membership == null)
            {
                throw new NotFoundException(
                    "Member membership not found.");
            }

            //Validate payment amount
            if (dto.Amount != membership.Amount)
            {
                throw new BadRequestException(
                    $"Payment amount must be {membership.Amount}.");
            }

            // Validate payment method
            var paymentMethod =
                dto.PaymentMethod.Trim();

            var allowedMethods = new[]
            {
                "Cash",
                "Card",
                "Online",
                "BankTransfer"
            };

            if (!allowedMethods.Contains(
                    paymentMethod,
                    StringComparer.OrdinalIgnoreCase))
            {
                throw new BadRequestException(
                    "Invalid payment method.");
            }

            // Normalize transaction reference
            var transactionReference =
                string.IsNullOrWhiteSpace(
                    dto.TransactionReference)
                    ? null
                    : dto.TransactionReference.Trim();

            //  Check duplicate transaction reference
            if (transactionReference != null)
            {
                var exists =
                    await _paymentRepository
                        .ExistsByTransactionReferenceAsync(
                            transactionReference);

                if (exists)
                {
                    throw new BadRequestException(
                        "Transaction reference already exists.");
                }
            }

            // Create payment
            var payment = new Payment
            {
                MemberMembershipId =
                    dto.MemberMembershipId,

                Amount = dto.Amount,

                PaymentDate = DateTime.UtcNow,

                PaymentMethod = paymentMethod,

                TransactionReference =
                    transactionReference,

                Remarks = string.IsNullOrWhiteSpace(dto.Remarks)
                    ? null
                    : dto.Remarks.Trim()
            };

            await _paymentRepository.AddAsync(payment);

            await _paymentRepository.SaveChangesAsync();

            // Return response
            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                MemberMembershipId =
                    membership.MemberMembershipId,

                MemberId = membership.MemberId,

                MemberName =
                    membership.Member.MemberName,

                PlanName =
                    membership.Plan.PlanName,

                Amount = payment.Amount,

                PaymentDate =
                    payment.PaymentDate,

                PaymentMethod =
                    payment.PaymentMethod,

                TransactionReference =
                    payment.TransactionReference,

                Remarks =
                    payment.Remarks
            };
        }

        public async Task<PaymentResponseDto>
            GetByIdAsync(int id)
        {
            var payment =
                await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
            {
                throw new NotFoundException(
                    "Payment not found.");
            }

            return MapToResponseDto(payment);
        }

        public async Task<PagedPaymentResponseDto>
            GetAllAsync(PaymentQueryDto query)
        {
            var payments =
                _paymentRepository.Query();

            // Member filter
            if (query.MemberId.HasValue)
            {
                payments = payments.Where(x =>
                    x.MemberMembership.MemberId ==
                    query.MemberId.Value);
            }

            // Membership filter
            if (query.MemberMembershipId.HasValue)
            {
                payments = payments.Where(x =>
                    x.MemberMembershipId ==
                    query.MemberMembershipId.Value);
            }

            // Payment method filter
            if (!string.IsNullOrWhiteSpace(
                    query.PaymentMethod))
            {
                payments = payments.Where(x =>
                    x.PaymentMethod ==
                    query.PaymentMethod);
            }

            // From date
            if (query.FromDate.HasValue)
            {
                payments = payments.Where(x =>
                    x.PaymentDate >= query.FromDate.Value);
            }

            // To date
            if (query.ToDate.HasValue)
            {
                payments = payments.Where(x =>
                    x.PaymentDate <= query.ToDate.Value);
            }

            // Sorting
            var sortBy =
                query.SortBy?.ToLower();

            var sortDescending =
                query.SortOrder?.ToLower() == "desc";

            payments = sortBy switch
            {
                "amount" =>
                    sortDescending
                        ? payments.OrderByDescending(x => x.Amount)
                        : payments.OrderBy(x => x.Amount),

                "paymentmethod" =>
                    sortDescending
                        ? payments.OrderByDescending(
                            x => x.PaymentMethod)
                        : payments.OrderBy(
                            x => x.PaymentMethod),

                "paymentid" =>
                    sortDescending
                        ? payments.OrderByDescending(
                            x => x.PaymentId)
                        : payments.OrderBy(
                            x => x.PaymentId),

                _ =>
                    sortDescending
                        ? payments.OrderByDescending(
                            x => x.PaymentDate)
                        : payments.OrderBy(
                            x => x.PaymentDate)
            };

            // Total records
            var totalRecords =
                await _paymentRepository
                    .GetTotalCountAsync(payments);

            // Pagination
            var paymentList =
                await _paymentRepository.GetPagedAsync(
                    payments,
                    query.PageNumber,
                    query.PageSize);

            var data = paymentList
                .Select(MapToResponseDto)
                .ToList();

            var totalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)query.PageSize);

            return new PagedPaymentResponseDto
            {
                Data = data,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                HasPreviousPage =
                    query.PageNumber > 1,
                HasNextPage =
                    query.PageNumber < totalPages
            };
        }

        private static PaymentResponseDto
            MapToResponseDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                PaymentId =
                    payment.PaymentId,

                MemberMembershipId =
                    payment.MemberMembershipId,

                MemberId =
                    payment.MemberMembership.MemberId,

                MemberName =
                    payment.MemberMembership
                        .Member.MemberName,

                PlanName =
                    payment.MemberMembership
                        .Plan.PlanName,

                Amount =
                    payment.Amount,

                PaymentDate =
                    payment.PaymentDate,

                PaymentMethod =
                    payment.PaymentMethod,

                TransactionReference =
                    payment.TransactionReference,

                Remarks =
                    payment.Remarks
            };
        }
    }
}