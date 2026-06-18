using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RIArrangement
    {
        // RI Band Attributes
        public int RIBand { get; set; }
        public int RIModelID { get; set; }
        public int ArrangementID { get; set; }

        // Main reinsurance details
        public decimal SumInsured { get; set; }
        public decimal ReserveToDate { get; set; }
        public decimal PaymentToDate { get; set; }
        public decimal ThisReserve { get; set; }
        public decimal ThisPayment { get; set; }
        public int CatastropheCodeID { get; set; }

        // XOL triggers
        public int XolClmModelID { get; set; }
        public decimal XolClmLimit { get; set; }
        public int XolCatModelID { get; set; }
        public decimal XolCatLimit { get; set; }
        public int XolCatReinstatements { get; set; }

        // Reinsurance lines
        private RIArrangementLine[] _RiLines;
        public RIArrangementLine[] RILines
        {
            get { return _RiLines; }
            set { _RiLines = value; }
        }
        public decimal Payment => PaymentToDate + ThisPayment;

        public decimal Reserve => ReserveToDate + ThisReserve;
        public int Round()
        {
            const double kTolerance = 0.01;

            decimal crDiff;

            // If we have no treaty array we have nothing to do!
            if (RILines == null)
            {
                // do nothing
            }
            else
            {
                // Initialise rows
                int lRounding = 0;
                int llBound = RILines.GetLowerBound(0);
                int lUBound = RILines.GetUpperBound(0);

                // Find best lines for allocating rounding amounts
                for (int lCount = llBound; lCount <= lUBound; lCount++)
                {
                    switch (RILines[lCount].Type)
                    {
                        case "R":
                            // First ret row is ideal, set rounding row and exit loop
                            lRounding = lCount;
                            break;
                        case "T":
                            // Keep track of first treaty in case no ret row is found
                            if (lRounding < lCount)
                            {
                                lRounding = lCount;
                            }
                            break;
                    }
                }

                // Check for tiny allocation discrepancies and sneak it into our rounding line
                crDiff = ThisReserve - TotalThisReserve;

                double dDiff = (double)crDiff;

                if (Math.Abs(dDiff) < kTolerance)
                {
                    RILines[lRounding].ThisReserve += crDiff;
                }

                // Check payment rounding
                crDiff = ThisPayment - TotalThisPayment;
                dDiff = (double)crDiff; // Update dDiff with the new crDiff value

                if (Math.Abs(dDiff) < kTolerance)
                {
                    RILines[lRounding].ThisPayment += crDiff;
                }
            }

            return 0; // Assuming the function should return 0 as the VB.NET function does
        }

        public decimal TotalThisReserve
        {
            get
            {
                int lCount;
                decimal cReserveValue = 0;

                // If we have a treaty array add up our values
                if (RILines != null)
                {
                    int lLBound = RILines.GetLowerBound(0);
                    int lUBound = RILines.GetUpperBound(0);

                    for (lCount = lLBound; lCount <= lUBound; lCount++)
                    {
                        cReserveValue += _RiLines[lCount].ThisReserve;
                    }
                }

                // Return total
                return cReserveValue;
            }
        }

        public decimal TotalThisPayment
        {
            get
            {
                int lCount;
                decimal cPaymentValue = 0;

                // If we have a treaty array add up our values
                if (RILines != null)
                {
                    int lLBound = RILines.GetLowerBound(0);
                    int lUBound = RILines.GetUpperBound(0);

                    for (lCount = lLBound; lCount <= lUBound; lCount++)
                    {
                        cPaymentValue += _RiLines[lCount].ThisPayment;
                    }
                }

                // Return total
                return cPaymentValue;
            }
        }


    }

}
