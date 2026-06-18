set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClaimPayments'
go
create view ClaimPayments as select
    Claim_Peril.claim_id                                    ClaimID,
    Reserve_Type.description                                Description,
    Reserve.initial_reserve                                 InitialReserve,
    Reserve.revised_reserve                                 RevisedReserve,
    (select amount
        from Claim_Payment where reserve_id = Reserve.reserve_id
        and date_of_payment = (select max(date_of_payment)
            from Claim_Payment
            where reserve_id = Reserve.reserve_id))         ThisPayment,
    Reserve.paid_to_date                                    PaidToDate,
    (
        (isnull(Reserve.initial_reserve, 0.0) * abs(Reserve.revised_reserve_entered - 1)) +
        (isnull(Reserve.revised_reserve, 0.0) * Reserve.revised_reserve_entered) -
        isnull(Reserve.paid_to_date, 0.0)
    )                                                       CurrentReserve
    from Claim_Peril
    inner join Reserve on Claim_Peril.claim_peril_id = Reserve.claim_peril_id
    inner join Reserve_Type on Reserve.reserve_type_id = Reserve_Type.reserve_type_id
go
