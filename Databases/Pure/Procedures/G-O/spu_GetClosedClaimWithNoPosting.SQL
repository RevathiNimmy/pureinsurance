DDLDropProcedure 'spu_GetClosedClaimWithNoPosting'
Go

CREATE PROCEDURE spu_GetClosedClaimWithNoPosting
AS
BEGIN

	exec DDLAddIndex 'stats_detail', 'stats_detail_type', 'stats_folder_cnt', 'this_premium_home'

	--********************** get all closed claims which match criteria below
	-- get all closed claims where the last transaction is not 'CLA' or 'CLP'
	-- or last transaction is 'CLA' or 'CLP' and transaction amount <> this_revision or transaction amount <> this_payment
	-- and its in the imbalanced closed claims set
	-- exclude claims with multiple perils and those with last posting is a clp with amount = this_revision

-- *********************************** add multiple perils closed claims with missing last transaction into #TmpMultiple (Start)*************************************	
	select c.claim_id, c.claim_number
    into #TmpTest
	from  claim c
	join claim_peril cp on c.claim_id = cp.claim_id
	left join 
	   (select sf2.*
		from stats_folder sf2
		join (select sf3.loss_id, max(sf3.stats_folder_cnt) stats_folder_cnt from stats_folder sf3 group by sf3.loss_id) msf
		on sf2.loss_id = msf.loss_id and sf2.stats_folder_cnt = msf.stats_folder_cnt
		where left(sf2.document_ref,3) in ('CLA', 'CLP')
		) sf on c.claim_id = sf.loss_id
	left join stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
	where c.claim_status_id = 3
	and 
	(
	sf.stats_folder_cnt is null
	or (sf.stats_folder_cnt is not null and sf.transaction_type_id <> 3 and sd.this_premium_original <> (select sum(r.this_revision) from reserve r where r.claim_peril_id = cp.claim_peril_id ))
	or (sf.stats_folder_cnt is not null and sf.transaction_type_id = 3 and sd.this_premium_original <> (select sum(r.this_payment) from reserve r where r.claim_peril_id = cp.claim_peril_id))
	)
	and c.claim_number in
	(
		--******************** closed claims with transactions do not sum to zero
		select  c.claim_number
		from    claim c 
		join    stats_folder sf on sf.loss_id = c.claim_id
		join    stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
		where   sd.stats_detail_type = 'GRS'
		and     sf.transaction_type_id in (1, 2, 3) -- open, adjustment, payment
		and     c.claim_status_id = 3
		group by 
			c.claim_number
		HAVING  sum(case when sf.transaction_type_id = 3 then sd.this_premium_home * -1 else sd.this_premium_home end) <> 0
	)
	AND ((SELECT IsNull(SUM(r.this_revision),0)FROM Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0
	OR (SELECT IsNull(SUM(r.this_payment),0) FROM Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0)
	group by c.claim_id, c.claim_number
	order by c.claim_number

	--********************** add multiperil claims to temp table *******************
	select claim_id, count(*) NoPeril
	into #TmpMultiPeril 
	from
	(
	select cp.claim_id, cp.claim_peril_id
	from claim_peril cp join reserve r on cp.claim_peril_id = r.claim_peril_id 
	where cp.claim_id in
	(select claim_id from #TmpTest)
	group by cp.claim_id, cp.claim_peril_id
	) test
	group by claim_id having count(*) > 1

	drop table #TmpTest
	
-- *********************************** add multiple perils closed claims with missing last transaction into #TmpMultiple (end) *************************************

-- ********************************** add closed claims with missing last transaction and last posting is a clp with amount equals to this_revision (start)*******************    
	select c.claim_id, c.claim_number
    into #TmpCLP
	from  claim c
	join claim_peril cp on c.claim_id = cp.claim_id
	left join 
	   (select sf2.*
		from stats_folder sf2
		join (select sf3.loss_id, max(sf3.stats_folder_cnt) stats_folder_cnt from stats_folder sf3 group by sf3.loss_id) msf
		on sf2.loss_id = msf.loss_id and sf2.stats_folder_cnt = msf.stats_folder_cnt
		where left(sf2.document_ref,3) in ('CLP')
		) sf on c.claim_id = sf.loss_id
	left join stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
	where c.claim_status_id = 3
	and sf.stats_folder_cnt is not null and sf.transaction_type_id = 3 and sd.this_premium_original = (select sum(r.this_revision) from reserve r where r.claim_peril_id = cp.claim_peril_id )
	and c.claim_number in
	(
		--******************** closed claims with transactions do not sum to zero
		select  c.claim_number
		from    claim c 
		join    stats_folder sf on sf.loss_id = c.claim_id
		join    stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
		where   sd.stats_detail_type = 'GRS'
		and     sf.transaction_type_id in (1, 2, 3) -- open, adjustment, payment
		and     c.claim_status_id = 3
		group by 
			c.claim_number
		HAVING  sum(case when sf.transaction_type_id = 3 then sd.this_premium_home * -1 else sd.this_premium_home end) <> 0
	)
	AND ((SELECT IsNull(SUM(r.this_revision),0)FROM Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0
	OR (SELECT IsNull(SUM(r.this_payment),0) FROM Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0)
	group by c.claim_id, c.claim_number
	order by c.claim_number
-- ********************************** add closed claims with missing last transaction and last posting is a clp and amount equals to this_revision (end)*******************    

-- *********** get all closed claims with missing last transaction excluding multi-peril and last posting of clp and amount equals this_revision (start)***********
	select c.claim_id, c.claim_number
	from  claim c
	join claim_peril cp on c.claim_id = cp.claim_id
	left join 
	   (select sf2.*
		from stats_folder sf2
		join (select sf3.loss_id, max(sf3.stats_folder_cnt) stats_folder_cnt from stats_folder sf3 group by sf3.loss_id) msf
		on sf2.loss_id = msf.loss_id and sf2.stats_folder_cnt = msf.stats_folder_cnt
		where left(sf2.document_ref,3) in ('CLA', 'CLP')
		) sf on c.claim_id = sf.loss_id
	left join stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
	where c.claim_status_id = 3
	and 
	(
	sf.stats_folder_cnt is null
	or (sf.stats_folder_cnt is not null and sf.transaction_type_id <> 3 and sd.this_premium_original <> (select sum(r.this_revision) from reserve r where r.claim_peril_id = cp.claim_peril_id ))
	or (sf.stats_folder_cnt is not null and sf.transaction_type_id = 3 and sd.this_premium_original <> (select sum(r.this_payment) from reserve r where r.claim_peril_id = cp.claim_peril_id))
	)
	and c.claim_number in
	(
		--******************** closed claims with transactions do not sum to zero
		select  c.claim_number
		from    claim c 
		join    stats_folder sf on sf.loss_id = c.claim_id
		join    stats_detail sd on sd.stats_folder_cnt = sf.stats_folder_cnt
		where   sd.stats_detail_type = 'GRS'
		and     sf.transaction_type_id in (1, 2, 3) -- open, adjustment, payment
		and     c.claim_status_id = 3
		group by 
			c.claim_number
		HAVING  sum(case when sf.transaction_type_id = 3 then sd.this_premium_home * -1 else sd.this_premium_home end) <> 0
	)
	and c.claim_id not in
	(select claim_id from #TmpCLP union select claim_id from #TmpMultiPeril)
	AND ((SELECT IsNull(SUM(r.this_revision),0)FROM Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0
	OR (SELECT IsNull(SUM(r.this_payment),0) FROM Reserve r WHERE r.claim_peril_id = cp.claim_peril_id) <> 0)
	group by c.claim_id, c.claim_number
	order by c.claim_id
-- *********** get all closed claims with missing last transaction excluding multi-peril and last posting of clp and amount equals this_revision (end)***********

    drop table #TmpCLP
    drop table #TmpMultiPeril
END
