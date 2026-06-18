SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_policy_section_insurers_sel'
GO

CREATE PROCEDURE spu_policy_section_insurers_sel
@InsuranceFileCnt INT,
@scheme_id INT,
@EffectiveDate DATETIME,
@cob_rating_section_id INT
AS  

CREATE TABLE #SectionTaxes  
(
	[type] [int] NOT NULL ,
	[party_cnt] [int] NOT NULL ,
	[Scheme] [int] NOT NULL ,
	[risk_group_id] [int] NOT NULL ,
	[risk_code_id] [int] NOT NULL ,
	[effective_date] [datetime] NOT NULL ,
	[rate1] [numeric](7, 4) NOT NULL ,
	[value1] [numeric](19, 4) NOT NULL ,
	[minimum_total1] [numeric](19, 4) NOT NULL ,
	[rate2] [numeric](7, 4) NOT NULL ,
	[value2] [numeric](19, 4) NOT NULL ,
	[minimum_total2] [numeric](19, 4) NOT NULL ,
	[rate3] [numeric](7, 4) NOT NULL ,
	[value3] [numeric](19, 4) NOT NULL ,
	[minimum_total3] [numeric](19, 4) NOT NULL ,
	[is_gemini_transferred] [tinyint] NULL ,
	[address_cnt] [int] NULL ,
	[Tax_Group_id] [int] NULL, 
	[insurer_name] varchar(255),
	[insurer_code] varchar(10)
) 

declare @risk_Code_id int
declare @risk_group_id int

select @risk_Code_id =risk_code_id from insurance_file where Insurance_file_cnt=@InsuranceFileCnt
select @risk_group_id=risk_group_id from risk_code where risk_code_id=@risk_Code_id


/*
Match order to what the rest of the system uses.
Start at scheme and if nothing there work from the lowest level up. 
    1)insurer_scheme_rate
    2)insurer_section_rate
    3)insurer_rate (code)
    4)insurer_group_rate
*/

--1)insurer_scheme_rate
INSERT INTO #SectionTaxes select 1,party_cnt,Scheme,@risk_group_id,@risk_code_id,effective_date,rate1,value1,minimum_total1,rate2,value2,minimum_total1,rate3,value3,minimum_total3,0,address_cnt,Tax_Group_id,'',''
                          from insurer_scheme_rate isr1 
                          where scheme=@scheme_id 
                          and effective_date =(select max(effective_date) from insurer_scheme_rate isr2 where isr2.party_cnt=isr1.party_cnt and isr2.effective_date<= getdate() )
                          and party_cnt not in (select party_cnt from #SectionTaxes)

--2)insurer_section_rate
INSERT INTO #SectionTaxes select 2,party_cnt,Scheme,-1,risk_code_id,effective_date,rate1,value1,minimum_total1,rate2,value2,minimum_total1,rate3,value3,minimum_total3,is_gemini_transferred,address_cnt,Tax_Group_id,'','' 
                          from insurer_section_rate isr1
                          where Risk_code_COB_Rating_Section_id=@cob_rating_section_id and risk_code_id=@risk_code_id 
                          and effective_date =(select max(effective_date) from insurer_section_rate isr2 where isr2.party_cnt=isr1.party_cnt and isr2.effective_date<= getdate() )

--3)insurer_rate (code)
INSERT INTO #SectionTaxes select 3,party_cnt,Scheme,@risk_group_id,@risk_code_id,effective_date,rate1,value1,minimum_total1,rate2,value2,minimum_total1,rate3,value3,minimum_total3,0,address_cnt,Tax_Group_id,'',''   
                          from insurer_rate ir1
                          where risk_code_id=@risk_code_id 
                          and effective_date =(select max(effective_date) from insurer_rate ir2 where ir2.party_cnt=ir1.party_cnt and ir2.effective_date<= getdate() )
                          and party_cnt not in (select party_cnt from #SectionTaxes)

--4)insurer_group_rate
INSERT INTO #SectionTaxes select 4,party_cnt,Scheme,-1,-1,effective_date,rate1,value1,minimum_total1,rate2,value2,minimum_total1,rate3,value3,minimum_total3,0,address_cnt,Tax_Group_id,'','' 
                          from insurer_group_rate igr1
                          where risk_group_id=@risk_group_id 
                          and effective_date =(select max(effective_date) from insurer_group_rate igr2 where igr2.party_cnt=igr1.party_cnt and igr2.effective_date<= getdate() )
                          and party_cnt not in (select party_cnt from #SectionTaxes)


--Fix up the insurer name and code
update #SectionTaxes set insurer_name=party.name, insurer_code=RTRIM(party.shortname)
       from party
       where party.party_cnt=#SectionTaxes.party_cnt

select * from #SectionTaxes
Drop TABLE #SectionTaxes  

GO
