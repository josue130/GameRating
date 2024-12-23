﻿
using GameRatingAPI.Entitie;
using GameRatingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace GameRatingAPI.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {

            _context = context;

        }
        public async Task<int> Add(Comment comment)
        {
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment.Id;
        }

        public async Task Delete(int id)
        {
            await _context.comments.Where(c => c.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.comments.AnyAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetAllComments(int gameId)
        {
            return await _context.comments.Where(c => c.GameId == gameId).ToListAsync();
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            return await _context.comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Comment comment)
        {
            _context.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
