using RServer.Web.Providers;
using RServer.Web.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RServer.Web
{
    public static class LocalFileHandlerExtensions
    {
        public static LocalFileHandler WithMimeService<T>(this LocalFileHandler handler) where T : IMimeService
        {
            return handler.WithMimeService(Activator.CreateInstance<T>());
        }

        public static LocalFileHandler WithMimeService(this LocalFileHandler handler, IMimeService service)
        {
            handler.MimeService = service;
            return handler;
        }
        public static LocalFileHandler WithAttachmentLogic<T>(this LocalFileHandler handler) where T : IAttachmentLogic
        {
            return handler.WithAttachmentLogic(Activator.CreateInstance<T>());
        }

        public static LocalFileHandler WithAttachmentLogic(this LocalFileHandler handler, IAttachmentLogic logic)
        {
            handler.AttachmentLogic = logic;
            return handler;
        }

        public static LocalFileHandler WithFileProvider<T>(this LocalFileHandler handler) where T : IFileProvider
        {
            return handler.WithFileProvider(Activator.CreateInstance<T>());
        }

        public static LocalFileHandler WithFileProvider(this LocalFileHandler handler, IFileProvider provider)
        {
            handler.AddProvider(provider);
            return handler;
        }
    }
}
