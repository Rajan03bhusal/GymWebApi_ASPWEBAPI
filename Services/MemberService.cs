using GymSystem.Dtos;
using GymSystem.Exceptions;
using GymSystem.Interfaces;
using GymSystem.Interfaces.IMember;
using GymSystem.Models;

namespace GymSystem.Services
{
    public class MemberService : IMemberService 
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // Create member
        public async Task<MemberResponseDto> CreteAsync(CreateMemberDto dto)
        {
            string email = dto.Email.Trim();
            string phone = dto.Phone.Trim();
            if (await _memberRepository.ExistsByEmailAsync(email))
            {
                throw new BadRequestException(
                      $"A member with this email '{email}' already exists."
                    );
            }
            if (await _memberRepository.ExistsByPhoneAsync(phone))
            {

                throw new BadRequestException(
                      $"A member with this phone '{phone}' already exists."
                    );
            }
            var member = new Member
            {
                MemberCode = GenerateMemberCode(),

                MemberName = dto.FullName.Trim(),

                Gender = dto.Gender.Trim(),

                DateOfBirth = dto.DateOfBirth,

                PhoneNumber = phone,

                Email = email,

                Address = dto.Address?.Trim(),

                JoinDate = DateTime.UtcNow,

                CreatedAt = DateTime.UtcNow
            };

            await _memberRepository.AddAsync(member);

            await _memberRepository.SaveChangesAsync();

            return MapToResponse(member);

        }

        // delete
        public async Task DeleteAsync(int memberId)
        {
            var member =
                           await _memberRepository.GetByIdAsync(memberId);

            if (member == null)
            {
                throw new NotFoundException(
                    $"Member with ID {memberId} was not found.");
            }
            await _memberRepository.DeleteAsync(member);

            await _memberRepository.SaveChangesAsync();
        }

        public async Task<PagedMemberResponseDto> GetAllAsync(MemberQueryDto query)
        {
            int totalrecords= await _memberRepository.GetTotalCountAsync(query);
            var members = await _memberRepository.GetPagedAsync(query);
            var data = members.Select(MapToResponse).ToList();

            int totalPages = (int)Math.Ceiling(totalrecords / (double)query.PageSize);
            return new PagedMemberResponseDto
            {
                Data = data,
                PageNumber=query.PageNumber,
                PageSize=query.PageSize,
                TotalRecords=totalrecords,
                TotalPages = totalPages,
                HasPreviousPage= query.PageSize > 1,
                HasNextPage= query.PageSize < 1,


            };
            
        }

        // Get By Id
        public async Task<MemberResponseDto> GetByIdAsync(int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if(member == null)
            {
                throw new NotFoundException(
                     $"Member with ID {memberId} was not found."
                );
            }
            return MapToResponse(member);
        }

        // update
        public async Task<MemberResponseDto> UpdateAsync(int memberId, UpdateMemberDto dto)
        {

            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null)
            {
                throw new NotFoundException(
                     $"Member with ID {memberId} was not found."
                );
            }
            string email = dto.Email.Trim();
            string phone = dto.Phone.Trim();

            var emailOwner = 
                await _memberRepository.GetByEmailAsync(email);
            if(emailOwner !=null && emailOwner.MemberId != memberId)
            {
                throw new BadRequestException(
                   "Another member already uses this email.");

            }
            var phoneOwner =
               await _memberRepository.GetByPhoneAsync(phone);

            if (phoneOwner != null &&
                phoneOwner.MemberId != memberId)
            {
                throw new BadRequestException(
                    "Another member already uses this phone number.");
            }
            member.MemberName = dto.FullName.Trim();

            member.PhoneNumber = phone;

            member.Email = email;

            member.Address = dto.Address?.Trim();

            await _memberRepository.UpdateAsync(member);

            await _memberRepository.SaveChangesAsync();

            return MapToResponse(member);
        }

        // MEMBER CODE
        private static string GenerateMemberCode()
        {
            return $"MEM-{Guid.NewGuid():N}"
                .Substring(0, 12)
                .ToUpper();
        }
        // MAPPING
        private static MemberResponseDto MapToResponse(
            Member member)
        {
            return new MemberResponseDto
            {
                MemberId = member.MemberId,

                MemberCode = member.MemberCode,

                FullName = member.MemberName,

                Gender = member.Gender,

                DateOfBirth = member.DateOfBirth,

                Phone = member.PhoneNumber,

                Email = member.Email,

                Address = member.Address,

                JoinDate = member.JoinDate

               
            };
        }

        }
    }
