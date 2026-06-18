namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateUserGroupUsersRequestTypeUsersRow
    {
        public int DisplaySequence { get; set; }
        public bool DisplaySequenceSpecified { get; set; }
        public bool IsSupervisor { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The UserKey field is required")]
        //
        public int UserKey { get; set; }
    }
}
