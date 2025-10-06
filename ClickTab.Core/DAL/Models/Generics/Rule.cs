using EQP.EFRepository.Core.Attributes;
using EQP.EFRepository.Core.Interface;
using ClickTab.Core.HelperService.LookupEntityService.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Generics
{
    [LookupClass(typeof(RuleLookupService), new string[] { "DescriptionEnum" }, IncludeFullObject = false)]
    public class Rule : IBaseEntity
    {
        public int ID { get; set; }

        public RulesDescriptionEnum DescriptionEnum { get; set; }

        public string UrlRoutes { get; set; }
        public List<RoleRule> RoleRules { get; set; } = new List<RoleRule>();
    }
    public enum RulesDescriptionEnum
    {
        NEW_HOSPITALIZATION = 1,
        DISCHARGED_PATIENTS=2,
        CONFIGURE_PROCEDURES=3,
        CONVENTION_VISITS=4,
        APA_PAC_MANAGE=5,
        HISTORY_PATIENT=6,
        REGISTRY_USERS = 11,
        REGISTRY_ROLES=12,
        REGISTRY_REGIONS =20,
        REGISTRY_LOCATIONS = 21,
        REGISTRY_MUNICIPALITIES = 22,
        REGISTRY_CITIZENSHIPS = 23,
        REGISTRY_SUPPLIERS = 24,
        REGISTRY_ARTICLES = 25,
        REGISTRY_MMG = 26,
        REGISTRY_DOCTOR_DEPARTMENT = 27,
        TABLES_DOCUMENTS_PS =40,
        TABLES_DEPARTMENTS = 41,
        SETTINGS=42,
        TRANSLATIONS=43,
        ORDERS=100,
        ORDER_AUTHORIZED=101,
        PROPOSAL_ORDERS = 102,
        VERIFY_ORDERS=103,
        WAREHOUSE=200,
        SYSTEM_NOTIFICATIONS = 300,
        STOCKS =201,
        STOCKS_UPDATE=202,
        REPORTING =500,
        REPORTING_FLOWS_APA_APC=501,
        REPORTING_DOCTOR_SUMMARY_APA_PAC = 502,
        REPORTING_SUMMARY_SDO = 503,
        REPORTING_PRINT_SDO = 504,

    }
    //public enum RulesDescriptionEnum
    //{
    //    CHAPTER_ICD10 = 1,
    //    PARAGRAPH_ICD10 = 2,
    //    PATHOLOGY_ICD10 = 3,
    //    LIBRARY_BRANCH = 4,
    //    LIBRARY_CLINIC = 5,
    //    REGISTRY_ROLES = 10,
    //    REGISTRY_USERS = 11,
    //    REGISTRY_FACILITIES = 12,
    //    REGISTRY_LOCATIONS = 13,
    //    REGISTRY_BRANCHES = 14,
    //    REGISTRY_SYMPTOMS = 15,
    //    REGISTRY_RISKFACTORS = 16,
    //    REGISTRY_OUTPATIENTCLINICS = 17,
    //    REGISTRY_TEMPLATES = 18,
    //    REGISTRY_BRANCHESLIBRARIES = 19,
    //    REGISTRY_CLINICLIBRARIES = 20,
    //    REGISTRY_PERFORMANCETYPES = 25,
    //    DOCTORSAREA_AVAILABILITIES = 100,
    //    DOCTORSAREA_CALENDAR = 101,
    //    DOCTORSAREA_VISITS = 102,
    //    DOCTORSAREA_DOCUMENTS = 103,
    //    PATIENTAREA_VISITS = 200,
    //    PATIENTAREA_RESERVEVISIT = 201,
    //    DOCTORSAREA_RESERVEVISIT = 202,
    //    DOCTORSAREA_PAYMENT = 203,
    //    PATIENTAREA_PAYMENT = 204,
    //    MYVISITS_REPORT = 302,
    //    MYVISITS_ATTACHMENT_STATUS = 303,
    //    PRESENTCASE = 304,
    //    TUTORING_MANAGEMENT = 400,
    //    DYNAMIC_MODULE = 500

    //}
}
