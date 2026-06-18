SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spe_Party_Insurer_sel'
GO


CREATE PROCEDURE spe_Party_Insurer_sel  
    @party_cnt int  
AS  
    
    SELECT  i.party_cnt,  
            i.agency_number,  
            i.binder_indicator,  
            i.report_indicator,  
            i.is_reinsurer,  
            i.reinsurance_type,  
            i.is_reinsurance_debit_credit_no,  
            i.default_comm_rate,  
            i.tax_group_id,  
            i.payment_method,  
            i.payment_frequency,  
            i.bank_account,  
            i.fsa_registration_number,  
            i.fsa_insurercreditrating_id,  
            i.fsa_insurerstatus_id,  
            i.is_retained,
            i.claims_rating_agency_id,
            i.claims_rating_grading,
            i.claims_rating_date,
            i.claims_rating_description,
            p.domiciled_for_tax,
            i.terms_of_payment_id,
            i.domiciled_for_tax,
            risk_transfer_agreement,
            i.Brokerlink_Subaccount,
            i.Brokerlink_UW_ID,
            i.is_ri_broker,
    	    i.insurer_locking_type_id,
            i.risk_transfer_editable,
            i.insurer_type_id,
            i.bureauaccountparty
    FROM    Party_Insurer i
    LEFT JOIN
            Party p On p.party_cnt = i.party_cnt
    WHERE   i.party_cnt = @party_cnt  

GO


