﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ServiceLayer
{
    public abstract class BaseEntity<T> where T : BaseEntity<T>
    {

        #region Properties
        [Key]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; } = DateTime.Now;

        public DateTime? LastUpdateDate { get; set; } = DateTime.Now;

        public string Description { get; set; } = string.Empty;

        #endregion

        #region Static GENERIC Crud Methods

        public static async Task<T> Get(int id, bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();

                var currentType = typeof(T);
                if (currentType.GetInterfaces().Contains(typeof(INonDeletable)))
                    set.Where(p => (p as INonDeletable).IsDeleted == false);

                if (includeFields)
                    return IncludeSet(set).FirstOrDefault(p => p.Id == id);

                return await set.FindAsync(id);
            }
        }

        public static async Task<List<T>> GetAll(bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();

                if (includeFields)
                    return await IncludeSet(set).ToListAsync();

                return await set.ToListAsync();
            }
        }
        public static async Task<List<T>> GetAll(QueryParameters queryParameters, bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();

                if (includeFields)
                    return await IncludeSet(set)
                        .Skip(queryParameters.Size * (queryParameters.Page - 1))
                        .Take(queryParameters.Size).ToListAsync();

                return await set
                    .Skip(queryParameters.Size * (queryParameters.Page - 1))
                   .Take(queryParameters.Size).ToListAsync();
            }
        }

        public static IQueryable<T> GetAllIQ(bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();

                if (includeFields)
                    return IncludeSet(set);

                return set;
            }
        }

        public static async Task<List<T>> GetAllDeleted(bool includeFields = false)
        {

            var currentType = typeof(T);
            if (!currentType.GetInterfaces().Contains(typeof(INonDeletable)))
                throw new Exception("Not a deletable class");

            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();


                if (includeFields)
                {
                    IncludeSet(set).Where(p => (p as INonDeletable).IsDeleted == true).ToList();
                }

                var query = set.Where(p => (p as INonDeletable).IsDeleted == true);

                return await query.ToListAsync();
            }
        }


        public static IQueryable<T> GetAllDeletedIQ(bool includeFields = false)
        {

            var currentType = typeof(T);
            if (!currentType.GetInterfaces().Contains(typeof(INonDeletable)))
                throw new Exception("Not a deletable class");

            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();


                if (includeFields)
                {
                    IncludeSet(set).Where(p => (p as INonDeletable).IsDeleted == true).ToList();
                }

                var query = set.Where(p => (p as INonDeletable).IsDeleted == true);

                return query;
            }
        }

        public static async Task<List<T>> GetAllNonDeleted(bool includeFields = false)
        {

            var currentType = typeof(T);
            if (!currentType.GetInterfaces().Contains(typeof(INonDeletable)))
                throw new Exception("Not a deletable class");

            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();


                if (includeFields)
                    return await IncludeSet(set).Where(p => (p as INonDeletable).IsDeleted == false).ToListAsync();

                return await set.Where(p => (p as INonDeletable).IsDeleted == false).ToListAsync();

            }
        }
        public static async Task<List<T>> GetAllNonDeleted(QueryParameters queryParameters, bool includeFields = false)
        {

            var currentType = typeof(T);
            if (!currentType.GetInterfaces().Contains(typeof(INonDeletable)))
                throw new Exception("Not a deletable class");

            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();


                if (includeFields)
                    return await IncludeSet(set)
                        .Where(p => (p as INonDeletable).IsDeleted == false)
                        .Skip(queryParameters.Size * (queryParameters.Page - 1))
                        .Take(queryParameters.Size)
                        .ToListAsync();

                return await set
                    .Where(p => (p as INonDeletable).IsDeleted == false)
                    .Skip(queryParameters.Size * (queryParameters.Page - 1))
                    .Take(queryParameters.Size)
                    .ToListAsync();

            }
        }

        public static IQueryable<T> GetAllNonDeletedIQ(QueryParameters queryParameters, bool includeFields = false)
        {

            var currentType = typeof(T);
            if (!currentType.GetInterfaces().Contains(typeof(INonDeletable)))
                throw new Exception("Not a deletable class");

            using (var context = new DatabaseContext())
            {
                var set = context.Set<T>();


                if (includeFields)
                    return IncludeSet(set).Where(p => (p as INonDeletable).IsDeleted == false);

                return set.Where(p => (p as INonDeletable).IsDeleted == false);

            }
        }
        public static async Task<bool> Insert(T data)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    context.Set<T>().Add(data);
                    data.CreationDate = DateTime.Now;
                    data.LastUpdateDate = DateTime.Now;
                    return await context.SaveChangesAsync() >= 1;
                }
                catch (Exception ex)
                {
                    //    throw;
                    return false;

                }


            }
        }

        public static async Task<List<T>> FindAll(Expression<Func<T, bool>> match, bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var set = context.Set<T>();

                    if (includeFields)
                        return await IncludeSet(set).Where(match).ToListAsync();

                    return await set.Where(match).ToListAsync();
                }
                catch (Exception ex)
                {
                    //        throw;
                    return null;

                }

            }

        }
        public static IQueryable<T> FindAllIQ(Expression<Func<T, bool>> match, bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var set = context.Set<T>();

                    if (includeFields)
                        return IncludeSet(set).Where(match);

                    return set.Where(match);
                }
                catch (Exception)
                {
                    //        throw;
                    return null;

                }

            }

        }

        //public static List<T> FindAllWithSpeceficFieldLoaded<TProperty>(Expression<Func<T, bool>> match, Expression<Func<T, TProperty>> speceficField) where TProperty : BaseEntity<TProperty>
        //{
        //    using (var context = new DatabaseContext())
        //    {
        //        try
        //        {
        //            var set = context.Set<T>();


        //            return set.Include(speceficField).Where(match).ToList();
        //        }
        //        catch (Exception)
        //        {
        //            //     throw;
        //            return null;

        //        }

        //    }

        //}

        //public static IQueryable<T> FindAllWithSpeceficFieldLoadedIQ<TProperty>(Expression<Func<T, bool>> match, Expression<Func<T, TProperty>> speceficField) where TProperty : BaseEntity<TProperty>
        //{
        //    using (var context = new DatabaseContext())
        //    {
        //        try
        //        {
        //            var set = context.Set<T>();


        //            return set.Include(speceficField).Where(match);
        //        }
        //        catch (Exception)
        //        {
        //            //     throw;
        //            return null;

        //        }

        //    }

        //}



        public static async Task<T> FindFirstOrDefault(Expression<Func<T, bool>> match, bool includeFields = false)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var set = context.Set<T>();

                    if (includeFields)
                        return await IncludeSet(set).FirstOrDefaultAsync(match); ;

                    return await set.FirstOrDefaultAsync(match);
                }
                catch (Exception ex)
                {
                    //      throw;
                    return null;

                }

            }
        }

        public static async Task<bool> Update(T updated, int key)
        {
            if (updated == null)
                return false;

            using (var context = new DatabaseContext())
            {
                try
                {
                    T existing = await context.Set<T>().FindAsync(key);
                    if (existing != null)
                    {
                        updated.LastUpdateDate = DateTime.Now;
                        context.Entry(existing).CurrentValues.SetValues(updated);
                        await context.SaveChangesAsync();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    //       throw;
                    return false;

                }

            }


        }

        public static async Task<bool> Delete(T entity)
        {
            using (var context = new DatabaseContext())
            {
                try
                {

                    T current = await context.Set<T>().FindAsync(entity.Id);

                    if (current == null)
                        return false;

                    if (current is INonDeletable)
                    {
                        (current as INonDeletable).IsDeleted = true;
                        await context.SaveChangesAsync();
                        return true;
                    }

                    context.Set<T>().Remove(current);
                    return await context.SaveChangesAsync() == 1;
                }
                catch (Exception)
                {
                    //       throw;
                    return false;

                }

            }
        }

        public static async Task<int> Count()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    return await context.Set<T>().CountAsync();
                }
                catch (Exception)
                {
                    return -1;
                }

            }

        }


        #endregion

        #region Helper

        private static IQueryable<T> IncludeSet(DbSet<T> set)
        {
            IQueryable<T> query;


            #region Model

            if (typeof(T) == typeof(Article))
            {
                query = set.Include(p => (p as Article).Writer)
                .Include(p => (p as Article).Category);

                return query;
            }

            #endregion



            throw new Exception("This Entity does not have any relationship!");

        }

        #endregion




    }
}