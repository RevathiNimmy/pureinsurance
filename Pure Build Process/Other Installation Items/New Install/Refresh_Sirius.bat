CD\

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllAccountDetails.sql" >>"C:\Pure\Dashboard\dashAllAccountDetails.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllClaimDetails.sql" >>"C:\Pure\Dashboard\dashAllClaimDetails.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllPartyDetails.sql" >>"C:\Pure\Dashboard\dashAllPartyDetails.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllTransactions.sql" >>"C:\Pure\Dashboard\dashAllTransactions.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllTransactionsOthers.sql" >>"C:\Pure\Dashboard\dashAllTransactionsOthers.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAddressContact.sql" >>"C:\Pure\Dashboard\dashAddressContact.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllClaimComments.sql" >>"C:\Pure\Dashboard\dashAllClaimComments.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\spu_dashAllUserDefinedRisks.sql" >>"C:\Pure\Dashboard\dashAllUserDefinedRisks.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllPolicies.sql" >>"C:\Pure\Dashboard\dashAllPolicies.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllPolicies_Earning.sql" >>"C:\Pure\Dashboard\dashAllPolicies_Earning.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashAllPolicies_Rating.sql" >>"C:\Pure\Dashboard\dashAllPolicies_Rating.log"

osql -U sirius -P $1R1U5 -S [DBSERVERNAME] -d [DBNAME] -i "C:\Pure\Dashboard\dashRisksScheme.sql" >>"C:\Pure\Dashboard\dashRisksScheme.log"

"C:\Program Files\QlikView\Qv.exe" "C:\Pure\Dashboard\Dashboard.qvw" /r 

copy "C:\Pure\Dashboard\Dashboard.qvw" "C:\ProgramData\QlikTech\Documents" /y 

EXIT