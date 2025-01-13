CREATE OR ALTER VIEW [vwGetIncomesAndExpenses] AS

	SELECT 
		t.UserId
		,MONTH(T.PaidOrReceivedAt) AS [Month]
		,YEAR(T.PaidOrReceivedAt) As [Year]
		,SUM(CASE WHEN T.Type = 1 THEN t.Amount ELSE 0 END) AS [Incomes]
		,SUM(CASE WHEN T.Type = 2 THEN t.Amount ELSE 0 END) AS [Expenses]
	FROM [Transactions] t
	WHERE 
		t.PaidOrReceivedAt >= DATEADD(MONTH, -12, CAST(GETDATE() AS DATE))
		AND t.PaidOrReceivedAt < DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))
	GROUP BY 
		t.UserId
		,MONTH(T.PaidOrReceivedAt)
		,YEAR(T.PaidOrReceivedAt);