﻿using Elkood.Web.Helper.ExtensionMethods.Boolean;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.SharedKernel.ExtensionMethods
{
    public static class ExtensionMethodsShared
    {
        public static object GetDictionaryDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(t => new { Key = t.ToString(), Id = (int)(object)t, Name = GetDisplayDescription(t) })
                       .Where(x => x.Name != string.Empty);
        }
        public static string GetDisplayDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                           ?.FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetDescription() ?? string.Empty;
        }
        public static string ToEmail(this string value, string inc = "SmartStart", string org = "me") => $"{value}@{inc}.{org}";
        public static string EnsureEmail(this string value) => new EmailAddressAttribute().IsValid(value).NestedIF(value, value.ToEmail());
        public static DateTime EndAcademicYear(int month = 9)
        {
            var local = DateTime.Now.ToLocalTime();
            if (local.Month < month)
                return new DateTime(local.Year, month, 1);
            return new DateTime(local.Year + 1, month, 1);
        }

        internal static readonly char[] chars =
          "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        public static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        ///TODO wait until Elkood.Web build in it
        public static void SolveCannotInsertDuplicateKeyUniqueIndex(Action action)
        {
            try
            {
                action();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2601))
            {
                SolveCannotInsertDuplicateKeyUniqueIndex(action);
                //please see https://github.com/Giorgi/EntityFramework.Exceptions/blob/master/EntityFramework.Exceptions.SqlServer/SqlServerExceptionProcessorStateManager.cs
                // 2601 whill not work with oracl or other un sql server . check for general number casting 
            }
        }

        ///TODO wait until Elkood.Web build in it
        public static async Task SolveCannotInsertDuplicateKeyUniqueIndexAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2601))
            {
                await SolveCannotInsertDuplicateKeyUniqueIndexAsync(action);
                //please see https://github.com/Giorgi/EntityFramework.Exceptions/blob/master/EntityFramework.Exceptions.SqlServer/SqlServerExceptionProcessorStateManager.cs
                // 2601 whill not work with oracl or other un sql server . check for general number casting 
            }
        }

    }
}
