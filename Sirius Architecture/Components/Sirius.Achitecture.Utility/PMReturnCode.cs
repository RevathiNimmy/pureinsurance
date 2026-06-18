namespace Sirius.Architecture.Utility {

    /// <summary>
    /// The return value from a Sirius COM method call.
    /// DO NOT USE THIS ENUMERATION FOR ANY OTHER PURPOSE!
    /// </summary>
    public enum PMReturnCode {

        // General
        // ********
        PMFalse = 0,
        PMTrue = 1,
        PMFail = 10,
        PMError = 11,
        PMSucceed = 12,
        PMOK = 20,
        PMCancel = 21,
        PMNavigate = 30,

        // Broking Link Set 1
        // RFC070498
        // *************
        PMMNoAuthority = 51,
        PMMAlreadyInUse = 52,
        PMMInvalidPassword = 53,
        PMMNoAccess = 54,

        // System
        // *******
        PMIncorrectUsername = 200,
        PMIncorrectPassword = 201,
        PMLoggedOnElsewhere = 202,
        PMLogError1 = 210,
        PMLogError2 = 211,
        PMMixedModeIncorrectUserName = 212,
        PMMixedModeUserLoggedOnElsewhere = 213,
        PMUnifiedModeIncorrectUserName = 214,
        PMUnifiedModeUserLoggedOnElsewhere = 215,

        // Interface
        // **********
        PMMoveStatusBack = 400,
        PMMoveStatusNext = 401,
        PMMoveStatusCancel = 402,
        PMMoveStatusFinish = 403,

        // Broking Link Set 2
        // RFC070498
        // *************
        PMError_argcount = 500,
        PMError_protocol = 501,
        PMError_notconnected = 502,
        PMError_timeout = 503,
        PMError_usage = 504,

        // Business
        // *********
        PMLogonExceeded = 600,
        PMLicenceExceeded = 601,
        PMInvalidLicenceKey = 602,
        PMDataChanged = 610,
        PMMandatoryMissing = 611,
        PMDataNotChanged = 612,
        PMInvalidRequest = 620,
        PMIncorrectDateFormat = 621,
        PMIncorrectSystemDate = 622,
        // RFC180299 New Constants Added for SA1.4
        PMEarlier = 623,
        PMLater = 624,
        PMInstallStarted = 625,
        // DAK121099 - New responses for Licencing
        PMBlockLicenceExceeded = 626,
        PMWarnLicenceExceeded = 627,
        PMInvalidRiskStatus = 628,

        // Navigator Return Codes
        // ***********************
        PMNavStartNewProcess = 700,
        PMNavCallComponent = 701,
        PMNavBuildMap = 702,
        PMNavRepeatMap = 703,
        PMNavEndMap = 704,
        PMNavNavigate = 705,
        PMNavEndProcess = 706,

        // Database
        // *********
        PMRecordChanged = 800,
        PMRecordDeleted = 801,
        PMRecordInUse = 810,
        PMNotFound = 811,
        PMBOF = 820,
        PMEOF = 821,
        PMQueryTimeout = 822,
        PMNonRaisedError = 823,
        PMDeadlock = 888,

        // Broking Link Set 3
        // RFC070498
        // *************
        PMNoHostRegistry = 1002,
        PMNoPortRegistry = 1003,
        PMNoConnection = 1004,
        PMNoPMLink = 1005,
        PMNoCompanies = 1006,

        // CTAF 20030722 start
        // Agents/Customers Online
        // ***********************
        PMNoEmailAddress = 1100,
        PMFailedEmail = 1101,
        PMUpdateUserFailed = 1102,
        PMUserNotExist = 1103,
        PMUserNotLinkedAgent = 1104,
        PMGISOutDated = 1105,

        // Documaster Errors
        PMDocumasterError = 1200,

        // RVH 07/06/2004
        // XML Serialize/Deserialize errors
        // *********************************
        PMXMLTooManyDimensions = 2101,
        PMXMLNotEnoughRows = 2102,
        PMXMLNotEnoughColumns = 2103,
        PMXMLParseError = 2104,

        PMBackOfficeError = 3000,
        PMBusinessRuleError = 3001,

        // DD 04/08/2005
        // Additional optional actions in Roadmap
        // ***************************************
        PMNavAction1 = 2200,
        PMNavAction2 = 2201,

        MandatoryInputMissing = 5001
    }
}
