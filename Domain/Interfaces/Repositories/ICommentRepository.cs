using api.Domain.Entities;

namespace api.Domain.Interfaces.Repositories;
public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
}