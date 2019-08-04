using AtlasSolar;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;


namespace CSCodeEvaler
{
    public class CSCodeEvaler
    {
        public object EvalCode()
        {
            string GeoServerAtlasSolar = AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\";
            string out_file_name_pure = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_FFFFFFF")}";
            string out_file_name = $"{out_file_name_pure}.tif";
            int OblastId = 11;
            GdalConfiguration.ConfigureGdal();
            string auto_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "auto_dist.tif");
            Dataset auto_dist_ds = Gdal.Open(auto_dist_file_name, Access.GA_ReadOnly);
            Band auto_dist_band = auto_dist_ds.GetRasterBand(1);
            int auto_dist_width = auto_dist_band.XSize;
            int auto_dist_height = auto_dist_band.YSize;
            float[] auto_dist_array = new float[auto_dist_width * auto_dist_height];
            auto_dist_band.ReadRaster(0, 0, auto_dist_width, auto_dist_height, auto_dist_array, auto_dist_width, auto_dist_height, 0, 0);
            double auto_dist_out_val;
            int auto_dist_out_hasval;
            float auto_dist_NoDataValue = -1;
            auto_dist_band.GetNoDataValue(out auto_dist_out_val, out auto_dist_out_hasval);
            if (auto_dist_out_hasval != 0)
                auto_dist_NoDataValue = (float)auto_dist_out_val;
            string kzcoveriskl_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "kzcoveriskl.tif");
            Dataset kzcoveriskl_ds = Gdal.Open(kzcoveriskl_file_name, Access.GA_ReadOnly);
            Band kzcoveriskl_band = kzcoveriskl_ds.GetRasterBand(1);
            int kzcoveriskl_width = kzcoveriskl_band.XSize;
            int kzcoveriskl_height = kzcoveriskl_band.YSize;
            byte[] kzcoveriskl_array = new byte[kzcoveriskl_width * kzcoveriskl_height];
            kzcoveriskl_band.ReadRaster(0, 0, kzcoveriskl_width, kzcoveriskl_height, kzcoveriskl_array, kzcoveriskl_width, kzcoveriskl_height, 0, 0);
            double kzcoveriskl_out_val;
            int kzcoveriskl_out_hasval;
            byte kzcoveriskl_NoDataValue = 255;
            kzcoveriskl_band.GetNoDataValue(out kzcoveriskl_out_val, out kzcoveriskl_out_hasval);
            if (kzcoveriskl_out_hasval != 0)
                kzcoveriskl_NoDataValue = (byte)kzcoveriskl_out_val;
            string lep_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "lep_dist.tif");
            Dataset lep_dist_ds = Gdal.Open(lep_dist_file_name, Access.GA_ReadOnly);
            Band lep_dist_band = lep_dist_ds.GetRasterBand(1);
            int lep_dist_width = lep_dist_band.XSize;
            int lep_dist_height = lep_dist_band.YSize;
            float[] lep_dist_array = new float[lep_dist_width * lep_dist_height];
            lep_dist_band.ReadRaster(0, 0, lep_dist_width, lep_dist_height, lep_dist_array, lep_dist_width, lep_dist_height, 0, 0);
            double lep_dist_out_val;
            int lep_dist_out_hasval;
            float lep_dist_NoDataValue = -1;
            lep_dist_band.GetNoDataValue(out lep_dist_out_val, out lep_dist_out_hasval);
            if (lep_dist_out_hasval != 0)
                lep_dist_NoDataValue = (float)lep_dist_out_val;
            string np_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "np_dist.tif");
            Dataset np_dist_ds = Gdal.Open(np_dist_file_name, Access.GA_ReadOnly);
            Band np_dist_band = np_dist_ds.GetRasterBand(1);
            int np_dist_width = np_dist_band.XSize;
            int np_dist_height = np_dist_band.YSize;
            float[] np_dist_array = new float[np_dist_width * np_dist_height];
            np_dist_band.ReadRaster(0, 0, np_dist_width, np_dist_height, np_dist_array, np_dist_width, np_dist_height, 0, 0);
            double np_dist_out_val;
            int np_dist_out_hasval;
            float np_dist_NoDataValue = -1;
            np_dist_band.GetNoDataValue(out np_dist_out_val, out np_dist_out_hasval);
            if (np_dist_out_hasval != 0)
                np_dist_NoDataValue = (float)np_dist_out_val;
            string ooptiskl_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "ooptiskl.tif");
            Dataset ooptiskl_ds = Gdal.Open(ooptiskl_file_name, Access.GA_ReadOnly);
            Band ooptiskl_band = ooptiskl_ds.GetRasterBand(1);
            int ooptiskl_width = ooptiskl_band.XSize;
            int ooptiskl_height = ooptiskl_band.YSize;
            byte[] ooptiskl_array = new byte[ooptiskl_width * ooptiskl_height];
            ooptiskl_band.ReadRaster(0, 0, ooptiskl_width, ooptiskl_height, ooptiskl_array, ooptiskl_width, ooptiskl_height, 0, 0);
            double ooptiskl_out_val;
            int ooptiskl_out_hasval;
            byte ooptiskl_NoDataValue = 255;
            ooptiskl_band.GetNoDataValue(out ooptiskl_out_val, out ooptiskl_out_hasval);
            if (ooptiskl_out_hasval != 0)
                ooptiskl_NoDataValue = (byte)ooptiskl_out_val;
            string slope_srtm_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "slope_srtm.tif");
            Dataset slope_srtm_ds = Gdal.Open(slope_srtm_file_name, Access.GA_ReadOnly);
            Band slope_srtm_band = slope_srtm_ds.GetRasterBand(1);
            int slope_srtm_width = slope_srtm_band.XSize;
            int slope_srtm_height = slope_srtm_band.YSize;
            float[] slope_srtm_array = new float[slope_srtm_width * slope_srtm_height];
            slope_srtm_band.ReadRaster(0, 0, slope_srtm_width, slope_srtm_height, slope_srtm_array, slope_srtm_width, slope_srtm_height, 0, 0);
            double slope_srtm_out_val;
            int slope_srtm_out_hasval;
            float slope_srtm_NoDataValue = -1;
            slope_srtm_band.GetNoDataValue(out slope_srtm_out_val, out slope_srtm_out_hasval);
            if (slope_srtm_out_hasval != 0)
                slope_srtm_NoDataValue = (float)slope_srtm_out_val;
            string srtm_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "srtm.tif");
            Dataset srtm_ds = Gdal.Open(srtm_file_name, Access.GA_ReadOnly);
            Band srtm_band = srtm_ds.GetRasterBand(1);
            int srtm_width = srtm_band.XSize;
            int srtm_height = srtm_band.YSize;
            int[] srtm_array = new int[srtm_width * srtm_height];
            srtm_band.ReadRaster(0, 0, srtm_width, srtm_height, srtm_array, srtm_width, srtm_height, 0, 0);
            double srtm_out_val;
            int srtm_out_hasval;
            int srtm_NoDataValue = -1;
            srtm_band.GetNoDataValue(out srtm_out_val, out srtm_out_hasval);
            if (srtm_out_hasval != 0)
                srtm_NoDataValue = (int)srtm_out_val;
            string swvdwnyear_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", OblastId.ToString() + "swvdwnyear.tif");
            Dataset swvdwnyear_ds = Gdal.Open(swvdwnyear_file_name, Access.GA_ReadOnly);
            Band swvdwnyear_band = swvdwnyear_ds.GetRasterBand(1);
            int swvdwnyear_width = swvdwnyear_band.XSize;
            int swvdwnyear_height = swvdwnyear_band.YSize;
            float[] swvdwnyear_array = new float[swvdwnyear_width * swvdwnyear_height];
            swvdwnyear_band.ReadRaster(0, 0, swvdwnyear_width, swvdwnyear_height, swvdwnyear_array, swvdwnyear_width, swvdwnyear_height, 0, 0);
            double swvdwnyear_out_val;
            int swvdwnyear_out_hasval;
            float swvdwnyear_NoDataValue = -1;
            swvdwnyear_band.GetNoDataValue(out swvdwnyear_out_val, out swvdwnyear_out_hasval);
            if (swvdwnyear_out_hasval != 0)
                swvdwnyear_NoDataValue = (float)swvdwnyear_out_val;
            // out tif file
            int min_width = Math.Min(
                auto_dist_width,
                Math.Min(kzcoveriskl_width,
                Math.Min(lep_dist_width,
                Math.Min(np_dist_width,
                Math.Min(ooptiskl_width,
                Math.Min(slope_srtm_width,
                Math.Min(srtm_width,
                swvdwnyear_width))))))),
            min_height = Math.Min(
                auto_dist_height,
                Math.Min(kzcoveriskl_height,
                Math.Min(lep_dist_height,
                Math.Min(np_dist_height,
                Math.Min(ooptiskl_height,
                Math.Min(slope_srtm_height,
                Math.Min(srtm_height,
                swvdwnyear_height)))))));
            Driver out_drv = Gdal.GetDriverByName("GTiff");
            int out_BlockXSize,
                out_BlockYSize;
            auto_dist_band.GetBlockSize(out out_BlockXSize, out out_BlockYSize);
            byte out_NoDataValue = 255;
            string[] out_options = new string[] { "BLOCKXSIZE=" + out_BlockXSize, "BLOCKYSIZE=" + out_BlockYSize, "NODATAVALUE=" + 255 };
            byte[] out_buffer_array = new byte[min_width * min_height];
            for (int i = min_width - 1; i >= 0; i--)
            {
                for (int j = min_height - 1; j >= 0; j--)
                {
                    if (auto_dist_array[i + j * auto_dist_width] == auto_dist_NoDataValue)
                    {
                        out_buffer_array[i + j * min_width] = out_NoDataValue;
                    }
                    else
                    {
                        float road = auto_dist_array[i + j * auto_dist_width];
                        out_buffer_array[i + j * min_width] = road < 20000F ? (byte)1 : (byte)0;
                    }
                }
            }
            out_file_name = Path.Combine(GeoServerAtlasSolar + "FindTerrain", out_file_name);
            Dataset out_ds = out_drv.Create(out_file_name, min_width, min_height, 1, DataType.GDT_Byte, out_options);
            Band out_band = out_ds.GetRasterBand(1);
            out_band.WriteRaster(0, 0, min_width, min_height, out_buffer_array, min_width, min_height, 0, 0);
            out_band.SetNoDataValue(out_NoDataValue);
            out_ds.SetProjection(auto_dist_ds.GetProjection());
            double[] out_pGT = new double[6];
            auto_dist_ds.GetGeoTransform(out_pGT);
            out_ds.SetGeoTransform(out_pGT);
            out_ds.SetGCPs(auto_dist_ds.GetGCPs(), "");
            out_band.FlushCache();
            out_ds.FlushCache();
            out_band.Dispose();
            out_ds.Dispose();
            auto_dist_array = null;
            kzcoveriskl_array = null;
            lep_dist_array = null;
            np_dist_array = null;
            ooptiskl_array = null;
            slope_srtm_array = null;
            srtm_array = null;
            swvdwnyear_array = null;
            GC.Collect();
            return true;
        }
    }
}