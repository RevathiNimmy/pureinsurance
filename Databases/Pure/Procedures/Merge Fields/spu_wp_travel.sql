SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_travel'
GO


CREATE PROCEDURE spu_wp_travel
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @scheme_type Varchar(255),
    @trip_option Varchar(255),
    @area Varchar(255),
    @is_baggage_and_money TINYINT OUTPUT,
    @name_of_insured Varchar(255),
    @age_of_insured INT OUTPUT,
    @is_premium_person TINYINT OUTPUT,
    @is_golf_cover TINYINT OUTPUT,
    @date_of_departure DateTime OUTPUT,
    @travel_period INT OUTPUT,
    @travel_period_qualifier Varchar(255)
AS


SELECT
    @scheme_type = travel.scheme_type ,
    @trip_option = travel.trip_option ,
    @area = travel.area ,
    @is_baggage_and_money = travel.is_baggage_and_money ,
    @name_of_insured = travel.name_of_insured ,
    @age_of_insured = travel.age_of_insured ,
    @is_premium_person = travel.is_premium_person ,
    @is_golf_cover = travel.is_golf_cover ,
    @date_of_departure = travel.date_of_departure ,
    @travel_period = travel.travel_period ,
    @travel_period_qualifier = travel.travel_period_qualifier

FROM travel
WHERE travel.insurance_file_cnt = @insurancefilecnt
GO


