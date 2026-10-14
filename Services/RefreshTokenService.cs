using Microsoft.EntityFrameworkCore;
using ticketManage.Data;
using ticketManage.Models;

namespace ticketManage.Services
{
    public class RefreshTokenService
    {
        private readonly AppDbContext _context;

        public RefreshTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> FindByUserId(string userId)
        {
            return await _context
                .RefreshTokens.Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> DeleteAllByUserId(string userId)
        {
            return await _context.RefreshTokens.Where(x => x.UserId == userId).ExecuteDeleteAsync();
        }

        public async Task<RefreshToken> RotateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await this.DeleteAllByUserId(refreshToken.UserId);
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }
    }
}
