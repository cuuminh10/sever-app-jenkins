using gmc_api.Base.dto.Product;
using gmc_api.Base.Helpers;
using System;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Base.dto
{
    public class QualityUtils
    {
        public static void SetStockQty(object objItems, string tableObjectName)
        {
            try
            {
                // String strItemTableName = GMCUtil.GetTableNameFromBusinessObject(objItems);
                var tablePrefixPropName = tableObjectName[0..^1];
                String strItemQtyColumnName = tablePrefixPropName + ColumnSuffix.cstItemQtyColumnSuffix;
                String strItemFactColumnName = tablePrefixPropName + ColumnSuffix.cstItemFactColumnSuffix;

                String strItemStkQtyColumnName = tablePrefixPropName + ColumnSuffix.cstItemStkQtyColumnSuffix;
                String strItemRQtyColumnName = tablePrefixPropName + ColumnSuffix.cstItemRemainQtyColumnSuffix;


                decimal dbItemQty = Convert.ToDecimal(Utils.GetPropertyValue(objItems, strItemQtyColumnName));
                decimal dbItemFactor = Convert.ToDecimal(Utils.GetPropertyValue(objItems, strItemFactColumnName));

                //Tinh them so luong kho dieu chinh 
                String strItemStkAdjQtyColumnName = tablePrefixPropName + ColumnSuffix.cstItemStkAdjQtyColumnSuffix;
                decimal dbItemStkAdjQty = 0.0M;
                object prop = Utils.GetPropertyValue(objItems, strItemStkAdjQtyColumnName);
                if (prop != null)
                    dbItemStkAdjQty = Convert.ToDecimal(prop);
                //) 

                //Vuong notes chua action, 19/3/2013,Nguyen yeu cau lay Qty/ItemFactor trong truong hop Phuoc Son, se review ky lai phan nay
                Utils.SetPropertyValue(objItems, strItemStkQtyColumnName, dbItemQty * dbItemFactor + dbItemStkAdjQty);
                //GMCDbUtil.SetPropertyValue(objItems, strItemStkQtyColumnName, dbItemQty / dbItemFactor);
                Utils.SetPropertyValue(objItems, strItemRQtyColumnName, dbItemQty);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void SetItemWeightAndVolumn(object objItems)
        {
            // int iICProductID = Convert.ToInt32(GMCDbUtil.GetPropertyValue(objItems, "FK_ICProductID"));
            // ICProductsInfo objICProductsInfo = (ICProductsInfo)new ICProductsController().GetObjectByID(iICProductID);
            //SetItemWeightAndVolumn(objItems, objICProductsInfo);
        }

        public static void SetItemWeightAndVolumn(object objItems, string tableObjectName, ICProductBasicInfo objICProductsInfo)
        {
            try
            {
                decimal dbWeight = 0;
                decimal dbVolumn = 0;

                decimal dbGrossWeight = 0;
                decimal dbNetWeight = 0;

                var tablePrefixPropName = tableObjectName.Substring(0, tableObjectName.Length - 1);

                String strItemStkQtyColumnName = tablePrefixPropName + ColumnSuffix.cstItemStkQtyColumnSuffix;


                String strItemWeightColumnName = tablePrefixPropName + ColumnSuffix.cstItemWeightColumnSuffix;
                String strItemVolumnColumnName = tablePrefixPropName + ColumnSuffix.cstItemVolumnColumnSuffix;

                String strItemGrossWeightColumnName = tablePrefixPropName + ColumnSuffix.cstItemGrossWeightColumnSuffix;
                String strItemNetWeightColumnName = tablePrefixPropName + ColumnSuffix.cstItemNetWeightColumnSuffix;

                String strItemUnitWeightColumnName = tablePrefixPropName + ColumnSuffix.cstItemUnitWeightColumnSuffix;
                String strItemUnitVolumnColumnName = tablePrefixPropName + ColumnSuffix.cstItemUnitVolumnColumnSuffix;


                decimal dbStkQty = Convert.ToDecimal(Utils.GetPropertyValue(objItems, strItemStkQtyColumnName));

                if (objICProductsInfo != null)
                {
                    decimal dbUnitWeight = objICProductsInfo.ICProductWeight;
                    decimal dbUnitVolumn = objICProductsInfo.ICProductVolume;

                    decimal dbUnitGrossWeight = objICProductsInfo.ICProductGrossWeight;
                    decimal dbUnitNetWeight = objICProductsInfo.ICProductNetWeight;

                    Utils.SetPropertyValue(objItems, strItemUnitWeightColumnName, dbUnitWeight);
                    Utils.SetPropertyValue(objItems, strItemUnitVolumnColumnName, dbUnitVolumn);

                    dbWeight = dbUnitWeight * dbStkQty;
                    dbVolumn = dbUnitVolumn * dbStkQty;

                    dbGrossWeight = dbUnitGrossWeight * dbStkQty;
                    dbNetWeight = dbUnitNetWeight * dbStkQty;
                }

                Utils.SetPropertyValue(objItems, strItemWeightColumnName, dbWeight);
                Utils.SetPropertyValue(objItems, strItemVolumnColumnName, dbVolumn);

                Utils.SetPropertyValue(objItems, strItemGrossWeightColumnName, dbGrossWeight);
                Utils.SetPropertyValue(objItems, strItemNetWeightColumnName, dbNetWeight);

            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
