
using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;

namespace WebApplication6.Repositories
    {
        public class EFCategoryRepository : ICategoryRepository
        {
            private readonly ApplicationDbContext _context;

            public EFCategoryRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            // Lấy tất cả các category
            public async Task<IEnumerable<Category>> GetAllAsync()
            {
                return await _context.Categories.ToListAsync();
            }

            // Lấy category theo ID
            public async Task<Category> GetByIdAsync(int id)
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            }

            // Thêm category mới
            public async Task AddAsync(Category category)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            // Cập nhật category
            public async Task UpdateAsync(Category category)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }

            // Xóa category theo ID
            public async Task DeleteAsync(int id)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }


