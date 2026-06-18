SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claim_peril_cud'
GO

CREATE PROCEDURE spu_claim_peril_cud  
    @claim_Peril_id int OUTPUT ,  
    @Claim_id int ,  
    @Peril_type_id int ,  
    @Description varchar(255) = NULL ,  
    @Comments varchar(255) = NULL ,  
    @sum_insured numeric(19, 4) ,  
    @ri_band int = NULL ,  
    @original_claim_Peril_id int = NULL ,  
    @gis_Screen_id int = NULL ,  
    @US tinyint  
AS  
BEGIN  
    IF @US = 0  
        RETURN  
    IF @US = 1  
        BEGIN  
        INSERT INTO claim_peril  
        (Claim_id ,  
         Peril_type_id ,  
         Description ,  
         Comments ,  
         sum_insured ,  
         ri_band ,  
         --original_claim_Peril_id,  
         gis_Screen_id)  
        VALUES  
        (@Claim_id ,  
         @Peril_type_id ,  
         @Description ,  
         @Comments ,  
         @sum_insured ,  
         @ri_band ,  
         --@original_claim_Peril_id,  
         @gis_screen_id)  
        SELECT @claim_Peril_id = @@IDENTITY  
    END  
    ELSE IF @US = 2  
        UPDATE claim_peril  
        SET  
            Claim_id=@Claim_id ,  
            Peril_type_id=@Peril_type_id ,  
            Description=@Description ,  
            Comments=@Comments ,  
            sum_insured=@sum_insured ,  
            ri_band=@ri_band ,  
            --original_claim_Peril_id=@original_claim_Peril_id,  
            gis_screen_id=isnull(@gis_screen_id,gis_screen_id) 
            WHERE claim_peril_id = @claim_peril_id  
        ELSE  
        DELETE  
        FROM claim_peril  
        WHERE claim_peril_id = @claim_peril_id  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
