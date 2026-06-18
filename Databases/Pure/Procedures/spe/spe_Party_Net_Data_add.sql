SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Net_Data_add'
GO

CREATE PROCEDURE spe_Party_Net_Data_add
    @party_cnt int,
    @password varchar(40),
    @mothers_maiden_name varchar(60),
    @tp_introducer_code char(10),
    @tp_user_code char(10),
    @memorable_date datetime,
    @a_question varchar(255),
    @the_answer varchar(255),
    @userid varchar(40),
    @current_ins_renewal_date datetime

AS

BEGIN
INSERT INTO Party_Net_Data (
    party_cnt ,
    password ,
    mothers_maiden_name ,
    tp_introducer_code ,
    tp_user_code ,
    memorable_date ,
    a_question ,
    the_answer ,
    userid ,
    current_ins_renewal_date )
VALUES (
    @party_cnt,
    @password,
    @mothers_maiden_name,
    @tp_introducer_code,
    @tp_user_code,
    @memorable_date,
    @a_question,
    @the_answer,
    @userid,
    @current_ins_renewal_date)
END
GO

