SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_convert_party'
GO

CREATE PROCEDURE spu_convert_party  
    @party_cnt int,  
    @party_type_id int,  
    @new_party_type_id int  
AS  
  
-----------------------------------------------------------------------------  
-- Function:    sp_convert_party  
-- Description: Converts a party from one type, to another  
-- History:     CTAF 220800 - Created  
--              CTAF 290800 - Create Lifestyle record for PC  
--              AMB  20-Oct-2003 - added trade_id to corporate client  
-----------------------------------------------------------------------------  
  
DECLARE @name varchar(60)  
DECLARE @party_business_id varchar(70)  
DECLARE @trade_code varchar(70)  
DECLARE @trading_since datetime  
DECLARE @employer_business varchar(70)  
DECLARE @secondary_employer_business varchar(70)  
  
    /*  
        Converts a party from one type to another  
  
        1 = personal  
        2 = group (not supported at present)  
        3 = agent (not supported at present)  
        4 = corporate  
    */  
  
    BEGIN TRANSACTION  
  
    IF (@new_party_type_id = 1) /* Convert to PC */  
    BEGIN  
  
        /* Switch the party_type_id */  
        UPDATE  Party  
        SET party_type_id = @new_party_type_id  
        WHERE   party_cnt = @party_cnt  
  
        /* If we're converting back then just use the record that's already there */  
        IF NOT EXISTS (SELECT * FROM Party_Personal_Client WHERE party_cnt = @party_cnt)  
        BEGIN  
  
            /* Default the forename to be the party's name, as its not nullable */  
            SELECT  @name = (SELECT name  
  
                     FROM Party  
                     WHERE party_cnt = @party_cnt)  
  
            /* Set employer_business to be the same as party_business_id */  
            SELECT @employer_business = (SELECT party_business_id  
                             FROM Party_Corporate_Client  
                             WHERE party_cnt = @party_cnt)  
  
            /* Same for secondary employer_business */  
            SELECT @secondary_employer_business = @employer_business  
  
            /* Create a blank Personal Client */  
            INSERT INTO Party_Personal_Client (  
                party_cnt,  
                party_title_code,  
                forename,  
                initials,  
                employment_status_code,  
                employer_cnt,  
  
                employer_business,  
                secondary_employer_business,  
                secondary_employment_status_co,  
  
                marital_status_code,  
                number_of_children,  
                Nationality_id,  
                country_of_origin_code,  
                mailshot,  
                is_pet_owner,  
                accommodation_type_code  
            ) VALUES (  
                @party_cnt,  
                NULL,  
                @name,  
                NULL,  
                NULL,  
                NULL,  
                @employer_business,  
                NULL,  
                NULL,  
                NULL,  
                NULL,  
                1,  
                NULL,  
                NULL,  
                NULL,  
  
                NULL  
            )  
  
            /* Do we need to create a lifestyle record? */  
            IF NOT EXISTS (SELECT * FROM Party_Lifestyle  
                       WHERE party_cnt = @party_cnt  
                       AND party_lifestyle_id = 1)  
            BEGIN  
                /* Create a lifestyle record for the party */  
                INSERT INTO Party_Lifestyle (  
                    party_cnt,  
                    party_lifestyle_id,  
                    name,  
                    category,  
                    date_of_birth,  
                    gender_code,  
                    occupation_code,  
                    secondary_occupation_code,  
                    is_smoker  
                ) VALUES (  
                    @party_cnt,  
                    1,  
                    @name,  
                    1,  
                    NULL,  
                    NULL,  
                    NULL,  
                    NULL,  
                    0  
                )  
            END  
  
        END  
  
    END  
    ELSE IF (@new_party_type_id = 4)  /* Convert to CC */  
    BEGIN  
  
        /* Switch the party_type_id */  
        UPDATE  Party  
  
        SET party_type_id = @new_party_type_id  
        WHERE   party_cnt = @party_cnt  
  
        /* If we're converting back then just use the record that's already there */  
        IF NOT EXISTS (SELECT * FROM Party_Corporate_Client WHERE party_cnt = @party_cnt)  
        BEGIN  
  
            /* Get the employer_business off the Party PC table */  
            SELECT @party_business_id = (SELECT employer_business  
                             FROM Party_Personal_Client  
                             WHERE party_cnt = @party_cnt)  
  
            /* May not be a party_business_id so check */  
            IF (@party_business_id IS NULL) SELECT @party_business_id = ''  
  
            /* Default Trade to the be the same */  
            SELECT @trade_code = @party_business_id  
  
            /* Set trading since to be the party's date of birth  */  
            SELECT @trading_since = IsNull(date_of_birth,'1899-12-29 00:00:00.000')  
                         FROM Party_Lifestyle  
                         WHERE party_cnt = @party_cnt  
                         AND party_lifestyle_id = 1  
  
            /* Create a blank Corporate Client */  
            INSERT INTO Party_Corporate_Client (  
                party_cnt,  
                company_reg,  
                trading_since_date,  
                party_business_id,  
                location,  
                no_of_offices,  
                no_of_employees,  
                financial_year,  
                trade_code,  
                wage_roll,  
                turnover,  
                --vat_code,  
                SIC_code_id,  
                salutation,    -- AMB 20-Oct-2003 : added  
                --VAT_exempt,    --                 : added  
                trade_id       --                 : added  
            ) VALUES (  
                @party_cnt,  
                '',  
                @trading_since,  
                @party_business_id,  
                0,  
                1,  
                0,  
                GetDate(),  
                @trade_code,  
                0,  
                0,  
                --'',  
                NULL,  
                '',  
                --0,  
                NULL  
            )  
  
        END  
  
    END  
  
    ELSE  
    BEGIN  
  
        RAISERROR ('Error: Party type not supported', 10, 1) WITH SETERROR  
  
    END  
  
    /* Commit or rollback the transaction */  
    IF (@@ERROR <> 0)  
        ROLLBACK TRANSACTION  
    ELSE  
        COMMIT TRANSACTION  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
