using api.Data.Context;
using api.Domain.Interfaces.Repositories;
using api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repository;
public class CommentRepository(MyContext context) : ICommentRepository
{

    private readonly MyContext _context = context;

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }
}