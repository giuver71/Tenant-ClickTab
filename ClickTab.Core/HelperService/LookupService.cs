using EQP.EFRepository.Core.Helpers;
using EQP.EFRepository.Core.Interface;
using EQP.EFRepository.Core.Models;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ClickTab.Core.HelperService;
using EQP.EFRepository.Core.Services;

namespace HSE.Core.HelperService
{
    public class EQPLookupServiceLocator : LookupServiceLocator<int>
    {
        public EQPLookupServiceLocator(DatabaseContext ctx) : base(ctx)
        {
        }

        public object NormalizeFullObject(object entity, LookupCustomConfig CustomConfig = null)
        {
            bool includeFullObject = CustomConfig == null ? UtilityHelper.GetLookupClassAttribute(entity.GetType()).IncludeFullObject : CustomConfig.IncludeFullObject;

            if (!includeFullObject)
                return null;
            else
            {
                //Per l'entità passata nel parametro mette a NULL tutte le proprietà che riguarda tipo custom o di tipo list
                List<PropertyInfo> customProperties = entity.GetType().GetProperties().Where(p => UtilityHelper.IsCustomClassCollection(p.PropertyType) || UtilityHelper.IsCustomClassType(p.PropertyType)).ToList();
                if (customProperties != null)
                {
                    foreach (PropertyInfo pInfo in customProperties)
                    {
                        entity.GetType().GetProperty(pInfo.Name).SetValue(entity, null);
                    }
                }
                return entity;
            }
        }
    }

    public class EQPLookupServiceStringLocator : LookupServiceLocator<string>
    {
        public EQPLookupServiceStringLocator(DatabaseContext ctx) : base(ctx)
        {
        }

        public object NormalizeFullObject(object entity, LookupCustomConfig CustomConfig = null)
        {
            bool includeFullObject = CustomConfig == null ? UtilityHelper.GetLookupClassAttribute(entity.GetType()).IncludeFullObject : CustomConfig.IncludeFullObject;

            if (!includeFullObject)
                return null;
            else
            {
                //Per l'entità passata nel parametro mette a NULL tutte le proprietà che riguarda tipo custom o di tipo list
                List<PropertyInfo> customProperties = entity.GetType().GetProperties().Where(p => UtilityHelper.IsCustomClassCollection(p.PropertyType) || UtilityHelper.IsCustomClassType(p.PropertyType)).ToList();
                if (customProperties != null)
                {
                    foreach (PropertyInfo pInfo in customProperties)
                    {
                        entity.GetType().GetProperty(pInfo.Name).SetValue(entity, null);
                    }
                }
                return entity;
            }
        }
    }
}
