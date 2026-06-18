using System;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserDetailsResponseType : BaseResponseType
    {
        //[DBCol("ConsolidatedAgentCommission")]
        public bool ConsolidatedAgentCommission { get; set; }
        //[DBCol("EmailAddress")]
        public string EmailAddress { get; set; }
        //[DBCol("FullUsername")]
        public string FullUsername { get; set; }
        //[DBCol("LastLogin")]
        public System.DateTime LastLogin { get; set; }
        public bool LastLoginSpecified { get; set; }
        //[DBCol("PartyCnt")]
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
        //[DBCol("PartyResolvedName")]
        public string PartyName { get; set; }
        //[DBCol("PartyShortName")]
        public string PartyShortName { get; set; }
        //[DBCol("PartyTypeCode")]
        public string PartyType { get; set; }
        public string PartyTypeId { get; set; }
        //[DBCol("PasswordChangedDate")]
        public System.DateTime PasswordChangeDate { get; set; }
        public bool PasswordChangeDateSpecified { get; set; }
        //[DBCol("Username")]
        public string PureUsername { get; set; }
        public int UserKey { get; set; }
        public System.Collections.Generic.List<BaseBranchType> SourceList { get; set; }
        public System.Collections.Generic.List<BaseGetUserDetailsResponseTypeRow> UserGroups { get; set; }
        public Uri UserGroupsFirstPage { get; set; }
        public Uri UserGroupsLastPage { get; set; }
        public Uri UserGroupsPreviousPage { get; set; }
        public Uri UserGroupsNextPage { get; set; }
        public int UserGroupsPageNumber { get; set; }
        public int UserGroupsPageSize { get; set; }
        public int UserGroupsTotalPages { get; set; }
        public int UserGroupsTotalRecords { get; set; }
        public Uri SourceListFirstPage { get; set; }
        public Uri SourceListLastPage { get; set; }
        public Uri SourceListPreviousPage { get; set; }
        public Uri SourceListNextPage { get; set; }
        public int SourceListPageNumber { get; set; }
        public int SourceListPageSize { get; set; }
        public int SourceListTotalPages { get; set; }
        public int SourceListTotalRecords { get; set; }


    }
}
