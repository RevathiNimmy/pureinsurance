SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_get_properties'
GO
/* eck081101 Set Paid Direct for Credit Card Postings */
CREATE PROCEDURE spu_pmb_trans_get_properties
    @insurance_file_cnt int,
    @last_trans_type_id int OUTPUT,
    @tax_amount         numeric(19, 4) OUTPUT,
    @commission_amount  numeric(19, 4) OUTPUT,
    @paid_direct        int OUTPUT,
    @agent_cnt          int OUTPUT,
    @subagent_cnt       int OUTPUT,
    @coinsurers_count   int OUTPUT,
    @policyshares_count int OUTPUT,
    @fees_count         int OUTPUT,
    @extras_count       int OUTPUT,
    @discount_count     int OUTPUT,
    @introducer_cnt     int OUTPUT
AS
BEGIN
/* Get amounts from Insurance File */

SELECT  @last_trans_type_id  = S.last_trans_type_id,
    @tax_amount = I.tax_amount + I.vat_amount,
    @commission_amount = I.commission_amount
FROM    Event_Insurance_File        I,
    Event_Insurance_File_System S
WHERE   I.Insurance_file_cnt = @insurance_file_cnt
AND I.Insurance_file_cnt = S.Insurance_file_cnt


/* Determine whether Paid Direct */
/*SELECT  @paid_direct = 0
SELECT  @paid_direct = 1
FROM    Event_Insurance_File    I
WHERE   I.Insurance_file_cnt = @insurance_file_cnt
AND (I.Payment_method = 'Direct Debit' OR I.Payment_method = 'Credit Card To Insurer' OR I.Payment_method = 'Direct To Insurer')
*/
SELECT @paid_direct = 0
SELECT @paid_direct = 1
FROM event_insurance_file I
INNER JOIN payment_method P ON P.description=I.payment_method
WHERE I.insurance_file_cnt = @insurance_file_cnt
AND P.direct_to_insurer=1
AND p.roll_up_tax_postings=1

--eck 220601 Agents and sub agents are now held in Policy Agent Table
/*SELECT    @agent_cnt = 0
SELECT  @agent_cnt = I.lead_agent_cnt
FROM    Event_Insurance_File        I,
    Event_Insurance_File_System S,
    Party               P,
    Party_agent         A,
    Party_Agent_Type        T
WHERE   I.Insurance_file_cnt = @insurance_file_cnt
AND I.Insurance_file_cnt = S.Insurance_file_cnt
AND I.lead_agent_cnt IS NOT NULL
AND I.lead_agent_cnt = P.party_cnt
AND A.party_cnt = P.party_cnt
AND     A.party_agent_type_id = T.party_agent_type_id
AND     (T.description = 'AGENT'
        OR T.description = 'Affinity')

SELECT  @subagent_cnt = 0

SELECT  @subagent_cnt = I.lead_agent_cnt
FROM    Event_Insurance_File        I,
    Event_Insurance_File_System S,
    Party               P,
    Party_agent         A,
    Party_Agent_Type        T
WHERE   I.Insurance_file_cnt = @insurance_file_cnt
AND I.Insurance_file_cnt = S.Insurance_file_cnt
AND I.lead_agent_cnt IS NOT NULL
AND I.lead_agent_cnt = P.party_cnt
AND A.party_cnt = P.party_cnt
AND     A.party_agent_type_id = T.party_agent_type_id
AND     T.description = 'SUB AGENT'
*/
--new code to get Agent and subagent count
SELECT  @agent_cnt = 0
SELECT @agent_cnt = COUNT (E.agent_cnt)
FROM    Event_Policy_agents     E,
     Party          P,
    Party_agent     A,
    Party_Agent_Type    T
WHERE E.Insurance_file_cnt = @insurance_file_cnt
AND  E.agent_cnt = P.party_cnt
AND  P.party_cnt = A.party_cnt
AND      A.party_agent_type_id = T.party_agent_type_id
AND      (T.description = 'AGENT'
         OR T.description = 'Affinity')

SELECT  @subagent_cnt = 0
SELECT @subagent_cnt = COUNT(E.agent_cnt)
FROM    Event_Policy_agents     E,
    Party           P,
    Party_agent     A,
    Party_Agent_Type        T
WHERE E.Insurance_file_cnt = @insurance_file_cnt
AND  E.agent_cnt = P.party_cnt
AND  P.party_cnt = A.party_cnt
AND      A.party_agent_type_id = T.party_agent_type_id
AND      T.description = 'SUB AGENT'

SELECT  @introducer_cnt = 0

SELECT @introducer_cnt = COUNT(E.agent_cnt)
FROM    Event_Policy_agents     E,
    Party           P,
    Party_agent     A,
    Party_Agent_Type        T
WHERE E.Insurance_file_cnt = @insurance_file_cnt
AND  E.agent_cnt = P.party_cnt
AND  P.party_cnt = A.party_cnt
AND      A.party_agent_type_id = T.party_agent_type_id
AND      T.description = 'INTRODUCER'

--end of new code 220101
SELECT @coinsurers_count = COUNT (C.party_cnt)
FROM    Event_Policy_coinsurers     C
WHERE   C.Insurance_file_cnt = @insurance_file_cnt

SELECT @policyshares_count = COUNT(H.party_cnt)
FROM    Event_policy_shared_premiums    H
WHERE   H.Insurance_file_cnt = @insurance_file_cnt

SELECT @fees_count = COUNT(F.party_cnt)
FROM    Event_policy_fee    F,
    Party           P,
    Party_type      T
WHERE   F.Insurance_file_cnt = @insurance_file_cnt
AND F.party_cnt  = P.Party_cnt
AND P.Party_type_id = T.Party_type_id
AND T.Code = 'FE'


SELECT @extras_count = COUNT(F.party_cnt)
FROM    Event_policy_fee    F,
    Party           P,
    Party_type      T
WHERE   F.Insurance_file_cnt = @insurance_file_cnt
AND F.party_cnt  = P.Party_cnt
AND P.Party_type_id = T.Party_type_id
AND T.Code = 'EX'

SELECT @discount_count = COUNT(F.party_cnt)
FROM    Event_policy_fee    F,
    Party           P,
    Party_type      T
WHERE   F.Insurance_file_cnt = @insurance_file_cnt
AND F.party_cnt  = P.Party_cnt
AND P.Party_type_id = T.Party_type_id
AND T.Code = 'DI'

END
GO
