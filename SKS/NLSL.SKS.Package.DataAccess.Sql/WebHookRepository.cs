using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    public class WebHookRepository : IWebHookRepository
    {
        private readonly PackageContext _context;
        private readonly ILogger<WebHookRepository> _logger;

        public WebHookRepository(PackageContext context, ILogger<WebHookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public long? Create(WebHook webhook)
        {
            try
            {
                _logger.LogDebug("starting, create webhook");
                _context.WebHooks.Add(webhook);
                _context.SaveChanges();
                _logger.LogDebug("create webhook complete");

                return webhook.Id;
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }

        public void Delete(long? id)
        {
            try
            {
                _logger.LogDebug("starting, delete webhook");
                WebHook temp = new()
                               {Id = id};

                _context.WebHooks.Remove(temp);
                _context.SaveChanges();
                _logger.LogDebug("delete webhook complete");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }


        public IList<WebHook> GetAllWebHooksByTrackingId(string id)
        {
            try
            {
                _logger.LogDebug("starting, get all webhook");
                IList<WebHook> webHook = _context.WebHooks.ToList().Where(x => x.trackingId == id).ToList();
                _logger.LogDebug("get all webhook complete");

                return webHook;
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }

        public WebHook? GetWebHookById(long? id)
        {
            try
            {
                _logger.LogDebug("starting, get webhook by id");
                WebHook? webHook = _context.WebHooks.ToList().FirstOrDefault(x => x.Id == id);
                _logger.LogDebug("get webhook complete");

                return webHook;
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }
    }
}