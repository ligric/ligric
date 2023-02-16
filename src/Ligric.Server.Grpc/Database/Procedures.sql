--DROP PROCEDURE IF EXISTS [GetUser]

--GO

--CREATE PROCEDURE [LoadSaleOrderItems]
--    @saleOrderIds AS [Ids] READONLY,
--    @ids AS [Ids] READONLY,
--	@isExpense BIT
--AS
--BEGIN
--	SET ARITHABORT ON;
--	SET NOCOUNT ON;

--	DECLARE @itemsParamCount INT;
--	SET @itemsParamCount = (SELECT COUNT(*) FROM @ids);

--    SELECT _saleOrderItem.[Id], _saleOrderItem.[Status], _saleOrderItem.[Tax], _saleOrderItem.[TaxCodeId], _saleOrderItem.[Consignment],
--            _saleOrderItem.[Amount], _saleOrderItem.[IsDropShip], _saleOrderItem.[Ordered], _saleOrderItem.[GroupId], _saleOrderItem.[OrderedInGroup],
--			_saleOrderItem.[IsCreateWorkOrder], _saleOrderItem.[ManuallyEnteredActualCost] AS [ActualCost], _item.[Name] AS [ItemName],
--			_saleOrderItem.[IsExpenseItem], _saleOrderItem.[SaleOrderId], _saleOrderItem.[NotReserveAllocationSubType],
--            _item.[ItemCost], _item.[Type] AS [ItemType], _saleOrderItem.[Rate], _item.Id AS [ItemId], _saleOrderItem.[PointOfSaleProcessingActionType],
--            _saleOrder.[Status] AS [SaleOrderStatus], _saleOrder.[Bucket] AS [SaleOrderBucket], _saleOrder.[SaleOrderType],
--			_saleOrderItem.[UnitOfMeasureId], _saleOrderItem.[InvoicedQty],
--			_saleOrderItem.[ShippedQty],
--            (SELECT SUM(_shipOrderItem.ShippingAvailableQty) FROM [ShipOrderItems] AS _shipOrderItem
--                            WHERE _shipOrderItem.[SaleOrderItemId] = _saleOrderItem.[Id]) AS [AvailableQty],
--            (SELECT SUM(_shipOrderItem.NotAvailableQty) FROM [ShipOrderItems] AS _shipOrderItem
--                            WHERE _shipOrderItem.[SaleOrderItemId] = _saleOrderItem.[Id]) AS [NonAvailableQty],
--		   (SELECT ISNULL(SUM([dbo].[ConvertQtyInMeasures](_inventory.[UnitOfMeasureId], _soItem.[UnitOfMeasureId], _inventory.[Qty])), 0)
--					FROM [Inventories] AS _inventory
--						LEFT JOIN [SaleOrderItems] AS _soItem ON _soItem.[Id] = _saleOrderItem.[Id]
--					WHERE _inventory.[IsAllocate] = 0 AND _inventory.[Deleted] = 0
--						AND(_saleOrderSettings.[CheckOtherStores] = 1 OR _saleOrder.[WarehouseId] IS NULL OR _inventory.[WarehouseId] = _saleOrder.[WarehouseId])
--						AND _inventory.[ItemId] = _saleOrderItem.[ItemId]) AS [RealAvailableQty],

--			CAST((SELECT COUNT(*) FROM [TenderOrderItems] AS _tenderOrderItem
--					INNER JOIN [TenderOrders] AS _tenderOrder ON _tenderOrderItem.[TenderOrderId] = _tenderOrder.[Id]
--                WHERE _tenderOrderItem.[SaleOrderItemId] = _saleOrderItem.[Id]
--					AND _tenderOrder.[Status] != 5 /* not draft */ AND _tenderOrder.[Status] != 2/* rejected */ 
--					AND _tenderOrder.[Status] != 3/*  closed */ AND _tenderOrderItem.[IsRequestedItem] = 1) AS BIT) AS [HasLinkedQuotes],

--            _saleOrderItem.[AllocationType] AS [AllocationType], _saleOrderItem.[IsProcessed], _saleOrderItem.[Index],
--			_workOrder.[Id] AS [AutoflowWorkOrderId],
--			_workOrder.[Status] AS [AutoflowWorkOrderStatus],

--			(SELECT TOP 1 _assemblyOrderItem.[Id] FROM [AssemblyOrderItems] AS _assemblyOrderItem 
--				WHERE _assemblyOrderItem.[SaleOrderItemId] = _saleOrderItem.[Id] AND _assemblyOrderItem.[GroupId] IS NOT NULL) AS [RelatedAssemblyOrderChildItemId],
--			(SELECT TOP 1 _assemblyOrderItem.[Status] FROM [AssemblyOrderItems] AS _assemblyOrderItem 
--				WHERE _assemblyOrderItem.[SaleOrderItemId] = _saleOrderItem.[Id] AND _assemblyOrderItem.[GroupId] IS NOT NULL) AS [RelatedAssemblyOrderChildItemStatus],

--			_saleOrderItem.[PriceLevelType] AS [PriceLevelType],
--			(SELECT COUNT(_userTaskItem.[Id]) 
--				FROM [UserTaskItems] AS _userTaskItem
--					INNER JOIN [UserTasks] AS _userTask ON _userTask.[Id] = _userTaskItem.[TaskId]
--				WHERE _userTaskItem.[EntityId] = _saleOrderItem.[Id] AND _userTask.[EntityType] = 0 /*Sale Orders*/ AND _userTask.[Status] != 2/*Completed*/) AS [OpenUserTasksCount],

--			_saleOrderItem.[DiscountAmount], _saleOrderItem.[DiscountRate], _saleOrderItem.[DiscountAppliedToGroup], _saleOrderItem.[DiscountTaxable],
--			_saleOrderItem.[VoidedByPurchaseOrderItem], _saleOrderItem.[CanCreatePoRequest],
--			CAST((SELECT COUNT(_purchaseOrderRequest.Id) 
--				FROM dbo.[PurchaseOrderRequests] AS _purchaseOrderRequest
--				 WHERE _purchaseOrderRequest.[SaleOrderItemId] = _saleOrderItem.[Id]) AS BIT) AS [HasPo],
--		   _saleOrderItem.[OrderItemGroupMode],
--		   _saleOrderItem.[CasePack], _saleOrderItem.[QtyCasePack], _saleOrderItem.[CaseRate], _saleOrderItem.[CasePerPallet], _saleOrderItem.[NumberOfPallets],
--		   _saleOrderItem.[CartonCBMRuleId], _saleOrderItem.[IsPickUpItem], _saleOrderItem.[Weight],
--		   _saleOrderItem.[RatePerPiece], _saleOrderItem.[PiecesQty], _saleOrderItem.[NegativeInventorySubStatus],
--		   CONVERT(BIT, IIF(_saleOrderItem.[GroupId] IS NULL AND _assemblyOrderItem.[Id] IS NOT NULL, 1, 0)) AS [HasAssemblyOrderItem],
--		   _saleOrderItem.[EstTimeOfArrival]
--    FROM [SaleOrderItems] _saleOrderItem
--		INNER JOIN @saleOrderIds _saleOrderId ON _saleOrderItem.[SaleOrderId] = _saleOrderId.[Id]
--		INNER JOIN [SaleOrders] _saleOrder ON _saleOrderItem.[SaleOrderId] = _saleOrder.[Id]
--		LEFT JOIN [SaleOrderSettings] _saleOrderSettings ON _saleOrderSettings.[SaleOrderId] = _saleOrder.[Id] 
--		LEFT JOIN [Items] _item ON _saleOrderItem.[ItemId] = _item.[Id]
--		LEFT JOIN [WorkOrderTasks] _workOrderTask ON _saleOrderItem.[Id] = _workOrderTask.[SaleOrderItemId]
--		LEFT JOIN [WorkOrders] _workOrder ON  _workOrderTask.[WorkOrderId] = _workOrder.[Id]
--		LEFT JOIN @ids AS _ids ON _saleOrderItem.[Id] = _ids.[Id]
--		LEFT JOIN [AssemblyOrderItems] _assemblyOrderItem ON _saleOrderItem.[Id] = _assemblyOrderItem.[SaleOrderItemId]
--    WHERE (@itemsParamCount = 0 OR _ids.[Id] IS NOT NULL)
--		AND _saleOrderItem.[IsExpenseItem] = @isExpense
--		AND _saleOrderItem.[Deleted] = 0
--	ORDER BY _saleOrderItem.[Index], _saleOrderItem.[Id]
--END	

--GO
