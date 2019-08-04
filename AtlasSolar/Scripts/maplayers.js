    // слой базовый
    var Layer_Base = new ol.layer.Tile({
        source: new ol.source.OSM()
});

// слой базовый сверху
var Layer_Base_Top = new ol.layer.Tile({
    source: new ol.source.OSM()
});

// источник и слой измерений
    var source = new ol.source.Vector();

    var vector = new ol.layer.Vector({
        source: source,
        style: new ol.style.Style({
            fill: new ol.style.Fill({
                color: 'rgba(255, 255, 255, 0.2)'
            }),
            stroke: new ol.style.Stroke({
                color: '#ffcc33',
                width: 2
            }),
            image: new ol.style.Circle({
                radius: 7,
                fill: new ol.style.Fill({
                    color: '#ffcc33'
                })
            })
        })
    });


// источник данных и слой AnalizeTerrain
var Source_AnalizeTerrain = new ol.source.TileWMS();
//var Source_AnalizeTerrain = new ol.source.TileWMS({
//    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
//    params: {
//        'LAYERS': 'AtlasSolar:2017_04_23_18_10_51_1124666',
//        'VERSION': '1.1.0',
//        'FORMAT': 'image/png',
//        'TILED': false
//    },
//    serverType: 'geoserver',
//    projection: 'EPSG:3857'
//});
//var Layer_AnalizeTerrain = new ol.layer.Tile({
//    extent: [8206036.825855993, 5511308.703170527, 8593732.096134793, 5899225.6402705265],
//    source: Source_AnalizeTerrain
//});
var Layer_AnalizeTerrain = new ol.layer.Tile({
    source: Source_AnalizeTerrain
});
//Layer_AnalizeTerrain.setOpacity(0.75);
Layer_AnalizeTerrain.setVisible(false);

// источник данных и слой FindTerrain
var Source_FindTerrain = new ol.source.TileWMS();
var Layer_FindTerrain = new ol.layer.Tile({
    source: Source_FindTerrain
});
Layer_FindTerrain.setVisible(false);


// векторный слой выбранных объектов
var Source_select = new ol.source.Vector({});
var Layer_select = new ol.layer.Vector({
    source: Source_select
});

// источник данных и слой oblasti
var Source_oblasti = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:oblasti2',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_oblasti = new ol.layer.Tile({
    source: Source_oblasti
});
Layer_oblasti.setVisible(false);

// источник данных и слой rayony
var Source_rayony = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:rayony2',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rayony = new ol.layer.Tile({
    source: Source_rayony
});
Layer_rayony.setVisible(false);

// источник данных, слой и стиль точек сравнения
function CPStyleFunction(feature, resolution) {
    var Id = feature.get('Id');
    if (Id == 1) {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 1,
                fill: new ol.style.Fill({ color: [150, 0, 0, 0.75] }),
                stroke: new ol.style.Stroke({ color: [255, 0, 0, 0.9], width: 5 })
            })
        });
    }
    if (Id == 2) {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 1,
                fill: new ol.style.Fill({ color: [0, 150, 0, 0.75] }),
                stroke: new ol.style.Stroke({ color: [0, 255, 0, 0.9], width: 5 })
            })
        });
    }
    if (Id == 3) {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 1,
                fill: new ol.style.Fill({ color: [0, 0, 150, 0.75] }),
                stroke: new ol.style.Stroke({ color: [0, 0, 255, 0.9], width: 5 })
            })
        });
    }
};
function CPSelectStyleFunction(feature, resolution) {
    var Id = feature.get('Id');
    if (Id == 1) {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 5,
                fill: new ol.style.Fill({ color: [255, 0, 0, 0.75] }),
                stroke: new ol.style.Stroke({ color: [255, 255, 0, 0.9], width: 2 })
            })
        });
    }
    if (Id == 2) {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 5,
                fill: new ol.style.Fill({ color: [0, 255, 0, 0.75] }),
                stroke: new ol.style.Stroke({ color: [255, 0, 0, 0.9], width: 2 })
            })
        });
    }
    if (Id == 3) {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 5,
                fill: new ol.style.Fill({ color: [0, 0, 255, 0.75] }),
                stroke: new ol.style.Stroke({ color: [255, 0, 0, 0.9], width: 2 })
            })
        });
    }
};
var Source_CP = new ol.source.Vector({});
var Layer_CP = new ol.layer.Vector({
    source: Source_CP,
    style: CPStyleFunction,
    renderBuffer: 200
});
//Layer_CP.setVisible(false);

// источник данных, слой и стиль СЭС
function SPPStyleFunction(feature, resolution) {
    var SPPStatusId = feature.get('SPPStatusId');
    var SPPPower = parseFloat(feature.get('Power'));
    var r = parseInt((3 + SPPPower / 15), 10);
    //if (SPPStatusId == 1) {
    //    return new ol.style.Style({
    //        image: new ol.style.Circle({
    //            radius: parseInt((3 + SPPPower / 15), 10),
    //            fill: new ol.style.Fill({ color: [255, 255, 0, 0.75] }),
    //            stroke: new ol.style.Stroke({ color: [255, 0, 0, 0.9], width: 1 })
    //        })
    //    });
    //}
    //else {
    //    return new ol.style.Style({
    //        image: new ol.style.Circle({
    //            radius: parseInt((3 + SPPPower / 15), 10),
    //            fill: new ol.style.Fill({ color: [200, 200, 200, 0.75] }),
    //            stroke: new ol.style.Stroke({ color: [0, 0, 255, 0.9], width: 1 })
    //        })
    //    });

    //}
    if (SPPStatusId == 1) {
        if (SPPPower < 50) {
            r = 10;
        }
        else {
            r = 16;
        }
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: r,
                fill: new ol.style.Fill({ color: [255, 255, 0, 0.75] }),
                stroke: new ol.style.Stroke({ color: [255, 0, 0, 0.9], width: 1 })
            })
        });
    }
    else {
        if (SPPPower < 14) {
            r = 10;
        }
        else if (SPPPower < 36) {
            r = 13;
        }
        else {
            r = 16;
        }
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: r,
                fill: new ol.style.Fill({ color: [200, 200, 200, 0.75] }),
                stroke: new ol.style.Stroke({ color: [0, 0, 255, 0.9], width: 1 })
            })
        });

    }
};
var SPPLayerJson = $("#SPPLayerJson").text();
var Source_spp = new ol.source.Vector({
    features: (new ol.format.GeoJSON()).readFeatures(SPPLayerJson, {
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857'
    })
});
var Layer_spp = new ol.layer.Vector({
    source: Source_spp,
    style: SPPStyleFunction,
    renderBuffer: 200
});
Layer_spp.setVisible(false);

// источник данных и слой lep
var Source_lep = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:lep3',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_lep = new ol.layer.Tile({
    source: Source_lep
});
Layer_lep.setOpacity(0.75);
Layer_lep.setVisible(false);

// источник данных и слой lep2
var Source_lep2 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:lep4',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_lep2 = new ol.layer.Tile({
    source: Source_lep2
});
Layer_lep2.setOpacity(0.75);
Layer_lep2.setVisible(false);

//// векторный слой lep
//var Layer_lep_v = new ol.layer.Vector();
//var url_lep_v = 'http://' + gip + ':8080/geoserver/AtlasSolar/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=AtlasSolar:lep&outputFormat=text/javascript&format_options=callback:getJson';
//$.ajax({
//    jsonp: false,
//    jsonpCallback: 'getJson',
//    type: 'GET',
//    url: url_lep_v,
//    async: false,
//    dataType: 'jsonp',
//    success: function (data_lep_v) {
//        Layer_lep_v = new ol.layer.Vector({
//            source: new ol.source.Vector({
//                features: (new ol.format.GeoJSON()).readFeatures(data_lep_v, {
//                    featureProjection: 'EPSG:3857'
//                })
//            })
//        });
//    }
//});

// источник данных и слой lep_bufer
var Source_lep_bufer = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:lep_bufer',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_lep_bufer = new ol.layer.Tile({
    source: Source_lep_bufer
});
Layer_lep_bufer.setOpacity(0.75);
Layer_lep_bufer.setVisible(false);

// источник данных и слой arheopamyat
var Source_arheopamyat = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:arheopamyat',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_arheopamyat = new ol.layer.Tile({
    source: Source_arheopamyat
});
Layer_arheopamyat.setOpacity(0.75);
Layer_arheopamyat.setVisible(false);

// источник данных и слой hidroohrzony
var Source_hidroohrzony = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:hidroohrzony',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_hidroohrzony = new ol.layer.Tile({
    source: Source_hidroohrzony
});
Layer_hidroohrzony.setOpacity(0.75);
Layer_hidroohrzony.setVisible(false);

// источник данных и слой pamyatnikprirodypol
var Source_pamyatnikprirodypol = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:pamyatnikprirodypol',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_pamyatnikprirodypol = new ol.layer.Tile({
    source: Source_pamyatnikprirodypol
});
Layer_pamyatnikprirodypol.setOpacity(0.75);
Layer_pamyatnikprirodypol.setVisible(false);

// источник данных и слой prirparki
var Source_prirparki = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:prirparki',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_prirparki = new ol.layer.Tile({
    source: Source_prirparki
});
Layer_prirparki.setOpacity(0.75);
Layer_prirparki.setVisible(false);

// источник данных и слой rezervaty
var Source_rezervaty = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:rezervaty',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rezervaty = new ol.layer.Tile({
    source: Source_rezervaty
});
Layer_rezervaty.setOpacity(0.75);
Layer_rezervaty.setVisible(false);

// источник данных и слой zakazniky
var Source_zakazniky = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:zakazniky',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_zakazniky = new ol.layer.Tile({
    source: Source_zakazniky
});
Layer_zakazniky.setOpacity(0.75);
Layer_zakazniky.setVisible(false);

// источник данных и слой zapovedniki
var Source_zapovedniki = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:zapovedniki',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_zapovedniki = new ol.layer.Tile({
    source: Source_zapovedniki
});
Layer_zapovedniki.setOpacity(0.75);
Layer_zapovedniki.setVisible(false);

// источник данных и слой zapovedzony
var Source_zapovedzony = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:zapovedzony',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_zapovedzony = new ol.layer.Tile({
    source: Source_zapovedzony
});
Layer_zapovedzony.setOpacity(0.75);
Layer_zapovedzony.setVisible(false);

// источник данных и слой srtm
var Source_srtm = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:srtm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srtm = new ol.layer.Tile({
    source: Source_srtm
});
Layer_srtm.setOpacity(0.75);
Layer_srtm.setVisible(false);

// источник данных и слой aspect_srtm
var Source_aspect_srtm = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:aspect_srtm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_aspect_srtm = new ol.layer.Tile({
    source: Source_aspect_srtm
});
Layer_aspect_srtm.setOpacity(0.75);
Layer_aspect_srtm.setVisible(false);

// источник данных и слой slope_srtm
var Source_slope_srtm = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:slope_srtm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_slope_srtm = new ol.layer.Tile({
    source: Source_slope_srtm
});
Layer_slope_srtm.setOpacity(0.75);
Layer_slope_srtm.setVisible(false);

// источник данных и слой avg_dnr_year
var Source_avg_dnr_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_year = new ol.layer.Tile({
    source: Source_avg_dnr_year
});
Layer_avg_dnr_year.setOpacity(0.75);
Layer_avg_dnr_year.setVisible(false);

// источник данных и слой avg_dnr_01
var Source_avg_dnr_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_01 = new ol.layer.Tile({
    source: Source_avg_dnr_01
});
Layer_avg_dnr_01.setOpacity(0.75);
Layer_avg_dnr_01.setVisible(false);

// источник данных и слой avg_dnr_02
var Source_avg_dnr_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_02 = new ol.layer.Tile({
    source: Source_avg_dnr_02
});
Layer_avg_dnr_02.setOpacity(0.75);
Layer_avg_dnr_02.setVisible(false);

// источник данных и слой avg_dnr_03
var Source_avg_dnr_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_03 = new ol.layer.Tile({
    source: Source_avg_dnr_03
});
Layer_avg_dnr_03.setOpacity(0.75);
Layer_avg_dnr_03.setVisible(false);

// источник данных и слой avg_dnr_04
var Source_avg_dnr_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_04 = new ol.layer.Tile({
    source: Source_avg_dnr_04
});
Layer_avg_dnr_04.setOpacity(0.75);
Layer_avg_dnr_04.setVisible(false);

// источник данных и слой avg_dnr_05
var Source_avg_dnr_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_05 = new ol.layer.Tile({
    source: Source_avg_dnr_05
});
Layer_avg_dnr_05.setOpacity(0.75);
Layer_avg_dnr_05.setVisible(false);

// источник данных и слой avg_dnr_06
var Source_avg_dnr_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_06 = new ol.layer.Tile({
    source: Source_avg_dnr_06
});
Layer_avg_dnr_06.setOpacity(0.75);
Layer_avg_dnr_06.setVisible(false);

// источник данных и слой avg_dnr_07
var Source_avg_dnr_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_07 = new ol.layer.Tile({
    source: Source_avg_dnr_07
});
Layer_avg_dnr_07.setOpacity(0.75);
Layer_avg_dnr_07.setVisible(false);

// источник данных и слой avg_dnr_08
var Source_avg_dnr_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_08 = new ol.layer.Tile({
    source: Source_avg_dnr_08
});
Layer_avg_dnr_08.setOpacity(0.75);
Layer_avg_dnr_08.setVisible(false);

// источник данных и слой avg_dnr_09
var Source_avg_dnr_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_09 = new ol.layer.Tile({
    source: Source_avg_dnr_09
});
Layer_avg_dnr_09.setOpacity(0.75);
Layer_avg_dnr_09.setVisible(false);

// источник данных и слой avg_dnr_10
var Source_avg_dnr_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_10 = new ol.layer.Tile({
    source: Source_avg_dnr_10
});
Layer_avg_dnr_10.setOpacity(0.75);
Layer_avg_dnr_10.setVisible(false);

// источник данных и слой avg_dnr_11
var Source_avg_dnr_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_11 = new ol.layer.Tile({
    source: Source_avg_dnr_11
});
Layer_avg_dnr_11.setOpacity(0.75);
Layer_avg_dnr_11.setVisible(false);

// источник данных и слой avg_dnr_12
var Source_avg_dnr_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:avg_dnr_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_dnr_12 = new ol.layer.Tile({
    source: Source_avg_dnr_12
});
Layer_avg_dnr_12.setOpacity(0.75);
Layer_avg_dnr_12.setVisible(false);

//var avg_dnr_layers = [
//    Layer_avg_dnr_01,
//    Layer_avg_dnr_02,
//    Layer_avg_dnr_03,
//    Layer_avg_dnr_04,
//    Layer_avg_dnr_05,
//    Layer_avg_dnr_06,
//    Layer_avg_dnr_07,
//    Layer_avg_dnr_08,
//    Layer_avg_dnr_09,
//    Layer_avg_dnr_10,
//    Layer_avg_dnr_11,
//    Layer_avg_dnr_12,
//    Layer_avg_dnr_year
//];
//var avg_dnr_sources = [
//    Source_avg_dnr_01,
//    Source_avg_dnr_02,
//    Source_avg_dnr_03,
//    Source_avg_dnr_04,
//    Source_avg_dnr_05,
//    Source_avg_dnr_06,
//    Source_avg_dnr_07,
//    Source_avg_dnr_08,
//    Source_avg_dnr_09,
//    Source_avg_dnr_10,
//    Source_avg_dnr_11,
//    Source_avg_dnr_12,
//    Source_avg_dnr_year
//];

// источник данных и слой swv_dwn_year
var Source_swv_dwn_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_year = new ol.layer.Tile({
    source: Source_swv_dwn_year
});
Layer_swv_dwn_year.setOpacity(0.75);
Layer_swv_dwn_year.setVisible(false);

// источник данных и слой swv_dwn_01
var Source_swv_dwn_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_01 = new ol.layer.Tile({
    source: Source_swv_dwn_01
});
Layer_swv_dwn_01.setOpacity(0.75);
Layer_swv_dwn_01.setVisible(false);

// источник данных и слой swv_dwn_02
var Source_swv_dwn_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_02 = new ol.layer.Tile({
    source: Source_swv_dwn_02
});
Layer_swv_dwn_02.setOpacity(0.75);
Layer_swv_dwn_02.setVisible(false);

// источник данных и слой swv_dwn_03
var Source_swv_dwn_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_03 = new ol.layer.Tile({
    source: Source_swv_dwn_03
});
Layer_swv_dwn_03.setOpacity(0.75);
Layer_swv_dwn_03.setVisible(false);

// источник данных и слой swv_dwn_04
var Source_swv_dwn_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_04 = new ol.layer.Tile({
    source: Source_swv_dwn_04
});
Layer_swv_dwn_04.setOpacity(0.75);
Layer_swv_dwn_04.setVisible(false);

// источник данных и слой swv_dwn_05
var Source_swv_dwn_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_05 = new ol.layer.Tile({
    source: Source_swv_dwn_05
});
Layer_swv_dwn_05.setOpacity(0.75);
Layer_swv_dwn_05.setVisible(false);

// источник данных и слой swv_dwn_06
var Source_swv_dwn_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_06 = new ol.layer.Tile({
    source: Source_swv_dwn_06
});
Layer_swv_dwn_06.setOpacity(0.75);
Layer_swv_dwn_06.setVisible(false);

// источник данных и слой swv_dwn_07
var Source_swv_dwn_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_07 = new ol.layer.Tile({
    source: Source_swv_dwn_07
});
Layer_swv_dwn_07.setOpacity(0.75);
Layer_swv_dwn_07.setVisible(false);

// источник данных и слой swv_dwn_08
var Source_swv_dwn_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_08 = new ol.layer.Tile({
    source: Source_swv_dwn_08
});
Layer_swv_dwn_08.setOpacity(0.75);
Layer_swv_dwn_08.setVisible(false);

// источник данных и слой swv_dwn_09
var Source_swv_dwn_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_09 = new ol.layer.Tile({
    source: Source_swv_dwn_09
});
Layer_swv_dwn_09.setOpacity(0.75);
Layer_swv_dwn_09.setVisible(false);

// источник данных и слой swv_dwn_10
var Source_swv_dwn_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_10 = new ol.layer.Tile({
    source: Source_swv_dwn_10
});
Layer_swv_dwn_10.setOpacity(0.75);
Layer_swv_dwn_10.setVisible(false);

// источник данных и слой swv_dwn_11
var Source_swv_dwn_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_11 = new ol.layer.Tile({
    source: Source_swv_dwn_11
});
Layer_swv_dwn_11.setOpacity(0.75);
Layer_swv_dwn_11.setVisible(false);

// источник данных и слой swv_dwn_12
var Source_swv_dwn_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:swv_dwn_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_swv_dwn_12 = new ol.layer.Tile({
    source: Source_swv_dwn_12
});
Layer_swv_dwn_12.setOpacity(0.75);
Layer_swv_dwn_12.setVisible(false);

// источник данных и слой exp_dif_year
var Source_exp_dif_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_year = new ol.layer.Tile({
    source: Source_exp_dif_year
});
Layer_exp_dif_year.setOpacity(0.75);
Layer_exp_dif_year.setVisible(false);

// источник данных и слой exp_dif_01
var Source_exp_dif_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_01 = new ol.layer.Tile({
    source: Source_exp_dif_01
});
Layer_exp_dif_01.setOpacity(0.75);
Layer_exp_dif_01.setVisible(false);

// источник данных и слой exp_dif_02
var Source_exp_dif_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_02 = new ol.layer.Tile({
    source: Source_exp_dif_02
});
Layer_exp_dif_02.setOpacity(0.75);
Layer_exp_dif_02.setVisible(false);

// источник данных и слой exp_dif_03
var Source_exp_dif_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_03 = new ol.layer.Tile({
    source: Source_exp_dif_03
});
Layer_exp_dif_03.setOpacity(0.75);
Layer_exp_dif_03.setVisible(false);

// источник данных и слой exp_dif_04
var Source_exp_dif_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_04 = new ol.layer.Tile({
    source: Source_exp_dif_04
});
Layer_exp_dif_04.setOpacity(0.75);
Layer_exp_dif_04.setVisible(false);

// источник данных и слой exp_dif_05
var Source_exp_dif_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_05 = new ol.layer.Tile({
    source: Source_exp_dif_05
});
Layer_exp_dif_05.setOpacity(0.75);
Layer_exp_dif_05.setVisible(false);

// источник данных и слой exp_dif_06
var Source_exp_dif_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_06 = new ol.layer.Tile({
    source: Source_exp_dif_06
});
Layer_exp_dif_06.setOpacity(0.75);
Layer_exp_dif_06.setVisible(false);

// источник данных и слой exp_dif_07
var Source_exp_dif_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_07 = new ol.layer.Tile({
    source: Source_exp_dif_07
});
Layer_exp_dif_07.setOpacity(0.75);
Layer_exp_dif_07.setVisible(false);

// источник данных и слой exp_dif_08
var Source_exp_dif_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_08 = new ol.layer.Tile({
    source: Source_exp_dif_08
});
Layer_exp_dif_08.setOpacity(0.75);
Layer_exp_dif_08.setVisible(false);

// источник данных и слой exp_dif_09
var Source_exp_dif_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_09 = new ol.layer.Tile({
    source: Source_exp_dif_09
});
Layer_exp_dif_09.setOpacity(0.75);
Layer_exp_dif_09.setVisible(false);

// источник данных и слой exp_dif_10
var Source_exp_dif_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_10 = new ol.layer.Tile({
    source: Source_exp_dif_10
});
Layer_exp_dif_10.setOpacity(0.75);
Layer_exp_dif_10.setVisible(false);

// источник данных и слой exp_dif_11
var Source_exp_dif_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_11 = new ol.layer.Tile({
    source: Source_exp_dif_11
});
Layer_exp_dif_11.setOpacity(0.75);
Layer_exp_dif_11.setVisible(false);

// источник данных и слой exp_dif_12
var Source_exp_dif_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:exp_dif_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_exp_dif_12 = new ol.layer.Tile({
    source: Source_exp_dif_12
});
Layer_exp_dif_12.setOpacity(0.75);
Layer_exp_dif_12.setVisible(false);

// источник данных и слой dni_year
var Source_dni_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_year = new ol.layer.Tile({
    source: Source_dni_year
});
Layer_dni_year.setOpacity(0.75);
Layer_dni_year.setVisible(false);

// источник данных и слой dni_01
var Source_dni_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_01 = new ol.layer.Tile({
    source: Source_dni_01
});
Layer_dni_01.setOpacity(0.75);
Layer_dni_01.setVisible(false);

// источник данных и слой dni_02
var Source_dni_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_02 = new ol.layer.Tile({
    source: Source_dni_02
});
Layer_dni_02.setOpacity(0.75);
Layer_dni_02.setVisible(false);

// источник данных и слой dni_03
var Source_dni_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_03 = new ol.layer.Tile({
    source: Source_dni_03
});
Layer_dni_03.setOpacity(0.75);
Layer_dni_03.setVisible(false);

// источник данных и слой dni_04
var Source_dni_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_04 = new ol.layer.Tile({
    source: Source_dni_04
});
Layer_dni_04.setOpacity(0.75);
Layer_dni_04.setVisible(false);

// источник данных и слой dni_05
var Source_dni_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_05 = new ol.layer.Tile({
    source: Source_dni_05
});
Layer_dni_05.setOpacity(0.75);
Layer_dni_05.setVisible(false);

// источник данных и слой dni_06
var Source_dni_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_06 = new ol.layer.Tile({
    source: Source_dni_06
});
Layer_dni_06.setOpacity(0.75);
Layer_dni_06.setVisible(false);

// источник данных и слой dni_07
var Source_dni_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_07 = new ol.layer.Tile({
    source: Source_dni_07
});
Layer_dni_07.setOpacity(0.75);
Layer_dni_07.setVisible(false);

// источник данных и слой dni_08
var Source_dni_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_08 = new ol.layer.Tile({
    source: Source_dni_08
});
Layer_dni_08.setOpacity(0.75);
Layer_dni_08.setVisible(false);

// источник данных и слой dni_09
var Source_dni_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_09 = new ol.layer.Tile({
    source: Source_dni_09
});
Layer_dni_09.setOpacity(0.75);
Layer_dni_09.setVisible(false);

// источник данных и слой dni_10
var Source_dni_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_10 = new ol.layer.Tile({
    source: Source_dni_10
});
Layer_dni_10.setOpacity(0.75);
Layer_dni_10.setVisible(false);

// источник данных и слой dni_11
var Source_dni_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_11 = new ol.layer.Tile({
    source: Source_dni_11
});
Layer_dni_11.setOpacity(0.75);
Layer_dni_11.setVisible(false);

// источник данных и слой dni_12
var Source_dni_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:dni_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_dni_12 = new ol.layer.Tile({
    source: Source_dni_12
});
Layer_dni_12.setOpacity(0.75);
Layer_dni_12.setVisible(false);

// источник данных и слой sic_year
var Source_sic_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_year = new ol.layer.Tile({
    source: Source_sic_year
});
Layer_sic_year.setOpacity(0.75);
Layer_sic_year.setVisible(false);

// источник данных и слой sic_01
var Source_sic_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_01 = new ol.layer.Tile({
    source: Source_sic_01
});
Layer_sic_01.setOpacity(0.75);
Layer_sic_01.setVisible(false);

// источник данных и слой sic_02
var Source_sic_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_02 = new ol.layer.Tile({
    source: Source_sic_02
});
Layer_sic_02.setOpacity(0.75);
Layer_sic_02.setVisible(false);

// источник данных и слой sic_03
var Source_sic_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_03 = new ol.layer.Tile({
    source: Source_sic_03
});
Layer_sic_03.setOpacity(0.75);
Layer_sic_03.setVisible(false);

// источник данных и слой sic_04
var Source_sic_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_04 = new ol.layer.Tile({
    source: Source_sic_04
});
Layer_sic_04.setOpacity(0.75);
Layer_sic_04.setVisible(false);

// источник данных и слой sic_05
var Source_sic_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_05 = new ol.layer.Tile({
    source: Source_sic_05
});
Layer_sic_05.setOpacity(0.75);
Layer_sic_05.setVisible(false);

// источник данных и слой sic_06
var Source_sic_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_06 = new ol.layer.Tile({
    source: Source_sic_06
});
Layer_sic_06.setOpacity(0.75);
Layer_sic_06.setVisible(false);

// источник данных и слой sic_07
var Source_sic_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_07 = new ol.layer.Tile({
    source: Source_sic_07
});
Layer_sic_07.setOpacity(0.75);
Layer_sic_07.setVisible(false);

// источник данных и слой sic_08
var Source_sic_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_08 = new ol.layer.Tile({
    source: Source_sic_08
});
Layer_sic_08.setOpacity(0.75);
Layer_sic_08.setVisible(false);

// источник данных и слой sic_09
var Source_sic_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_09 = new ol.layer.Tile({
    source: Source_sic_09
});
Layer_sic_09.setOpacity(0.75);
Layer_sic_09.setVisible(false);

// источник данных и слой sic_10
var Source_sic_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_10 = new ol.layer.Tile({
    source: Source_sic_10
});
Layer_sic_10.setOpacity(0.75);
Layer_sic_10.setVisible(false);

// источник данных и слой sic_11
var Source_sic_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_11 = new ol.layer.Tile({
    source: Source_sic_11
});
Layer_sic_11.setOpacity(0.75);
Layer_sic_11.setVisible(false);

// источник данных и слой sic_12
var Source_sic_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sic_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sic_12 = new ol.layer.Tile({
    source: Source_sic_12
});
Layer_sic_12.setOpacity(0.75);
Layer_sic_12.setVisible(false);

// источник данных и слой sid_year
var Source_sid_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_year = new ol.layer.Tile({
    source: Source_sid_year
});
Layer_sid_year.setOpacity(0.75);
Layer_sid_year.setVisible(false);

// источник данных и слой sid_01
var Source_sid_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_01 = new ol.layer.Tile({
    source: Source_sid_01
});
Layer_sid_01.setOpacity(0.75);
Layer_sid_01.setVisible(false);

// источник данных и слой sid_02
var Source_sid_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_02 = new ol.layer.Tile({
    source: Source_sid_02
});
Layer_sid_02.setOpacity(0.75);
Layer_sid_02.setVisible(false);

// источник данных и слой sid_03
var Source_sid_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_03 = new ol.layer.Tile({
    source: Source_sid_03
});
Layer_sid_03.setOpacity(0.75);
Layer_sid_03.setVisible(false);

// источник данных и слой sid_04
var Source_sid_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_04 = new ol.layer.Tile({
    source: Source_sid_04
});
Layer_sid_04.setOpacity(0.75);
Layer_sid_04.setVisible(false);

// источник данных и слой sid_05
var Source_sid_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_05 = new ol.layer.Tile({
    source: Source_sid_05
});
Layer_sid_05.setOpacity(0.75);
Layer_sid_05.setVisible(false);

// источник данных и слой sid_06
var Source_sid_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_06 = new ol.layer.Tile({
    source: Source_sid_06
});
Layer_sid_06.setOpacity(0.75);
Layer_sid_06.setVisible(false);

// источник данных и слой sid_07
var Source_sid_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_07 = new ol.layer.Tile({
    source: Source_sid_07
});
Layer_sid_07.setOpacity(0.75);
Layer_sid_07.setVisible(false);

// источник данных и слой sid_08
var Source_sid_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_08 = new ol.layer.Tile({
    source: Source_sid_08
});
Layer_sid_08.setOpacity(0.75);
Layer_sid_08.setVisible(false);

// источник данных и слой sid_09
var Source_sid_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_09 = new ol.layer.Tile({
    source: Source_sid_09
});
Layer_sid_09.setOpacity(0.75);
Layer_sid_09.setVisible(false);

// источник данных и слой sid_10
var Source_sid_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_10 = new ol.layer.Tile({
    source: Source_sid_10
});
Layer_sid_10.setOpacity(0.75);
Layer_sid_10.setVisible(false);

// источник данных и слой sid_11
var Source_sid_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_11 = new ol.layer.Tile({
    source: Source_sid_11
});
Layer_sid_11.setOpacity(0.75);
Layer_sid_11.setVisible(false);

// источник данных и слой sid_12
var Source_sid_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sid_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sid_12 = new ol.layer.Tile({
    source: Source_sid_12
});
Layer_sid_12.setOpacity(0.75);
Layer_sid_12.setVisible(false);

// источник данных и слой sis_year
var Source_sis_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_year = new ol.layer.Tile({
    source: Source_sis_year
});
Layer_sis_year.setOpacity(0.75);
Layer_sis_year.setVisible(false);

// источник данных и слой sis_01
var Source_sis_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_01 = new ol.layer.Tile({
    source: Source_sis_01
});
Layer_sis_01.setOpacity(0.75);
Layer_sis_01.setVisible(false);

// источник данных и слой sis_02
var Source_sis_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_02 = new ol.layer.Tile({
    source: Source_sis_02
});
Layer_sis_02.setOpacity(0.75);
Layer_sis_02.setVisible(false);

// источник данных и слой sis_03
var Source_sis_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_03 = new ol.layer.Tile({
    source: Source_sis_03
});
Layer_sis_03.setOpacity(0.75);
Layer_sis_03.setVisible(false);

// источник данных и слой sis_04
var Source_sis_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_04 = new ol.layer.Tile({
    source: Source_sis_04
});
Layer_sis_04.setOpacity(0.75);
Layer_sis_04.setVisible(false);

// источник данных и слой sis_05
var Source_sis_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_05 = new ol.layer.Tile({
    source: Source_sis_05
});
Layer_sis_05.setOpacity(0.75);
Layer_sis_05.setVisible(false);

// источник данных и слой sis_06
var Source_sis_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_06 = new ol.layer.Tile({
    source: Source_sis_06
});
Layer_sis_06.setOpacity(0.75);
Layer_sis_06.setVisible(false);

// источник данных и слой sis_07
var Source_sis_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_07 = new ol.layer.Tile({
    source: Source_sis_07
});
Layer_sis_07.setOpacity(0.75);
Layer_sis_07.setVisible(false);

// источник данных и слой sis_08
var Source_sis_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_08 = new ol.layer.Tile({
    source: Source_sis_08
});
Layer_sis_08.setOpacity(0.75);
Layer_sis_08.setVisible(false);

// источник данных и слой sis_09
var Source_sis_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_09 = new ol.layer.Tile({
    source: Source_sis_09
});
Layer_sis_09.setOpacity(0.75);
Layer_sis_09.setVisible(false);

// источник данных и слой sis_10
var Source_sis_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_10 = new ol.layer.Tile({
    source: Source_sis_10
});
Layer_sis_10.setOpacity(0.75);
Layer_sis_10.setVisible(false);

// источник данных и слой sis_11
var Source_sis_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_11 = new ol.layer.Tile({
    source: Source_sis_11
});
Layer_sis_11.setOpacity(0.75);
Layer_sis_11.setVisible(false);

// источник данных и слой sis_12
var Source_sis_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:sis_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_12 = new ol.layer.Tile({
    source: Source_sis_12
});
Layer_sis_12.setOpacity(0.75);
Layer_sis_12.setVisible(false);

// источник данных и слой t10m_01
var Source_t10m_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_01 = new ol.layer.Tile({
    source: Source_t10m_01
});
Layer_t10m_01.setOpacity(0.75);
Layer_t10m_01.setVisible(false);

// источник данных и слой t10m_02
var Source_t10m_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_02 = new ol.layer.Tile({
    source: Source_t10m_02
});
Layer_t10m_02.setOpacity(0.75);
Layer_t10m_02.setVisible(false);

// источник данных и слой t10m_03
var Source_t10m_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_03 = new ol.layer.Tile({
    source: Source_t10m_03
});
Layer_t10m_03.setOpacity(0.75);
Layer_t10m_03.setVisible(false);

// источник данных и слой t10m_04
var Source_t10m_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_04 = new ol.layer.Tile({
    source: Source_t10m_04
});
Layer_t10m_04.setOpacity(0.75);
Layer_t10m_04.setVisible(false);

// источник данных и слой t10m_05
var Source_t10m_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_05 = new ol.layer.Tile({
    source: Source_t10m_05
});
Layer_t10m_05.setOpacity(0.75);
Layer_t10m_05.setVisible(false);

// источник данных и слой t10m_06
var Source_t10m_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_06 = new ol.layer.Tile({
    source: Source_t10m_06
});
Layer_t10m_06.setOpacity(0.75);
Layer_t10m_06.setVisible(false);

// источник данных и слой t10m_07
var Source_t10m_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_07 = new ol.layer.Tile({
    source: Source_t10m_07
});
Layer_t10m_07.setOpacity(0.75);
Layer_t10m_07.setVisible(false);

// источник данных и слой t10m_08
var Source_t10m_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_08 = new ol.layer.Tile({
    source: Source_t10m_08
});
Layer_t10m_08.setOpacity(0.75);
Layer_t10m_08.setVisible(false);

// источник данных и слой t10m_09
var Source_t10m_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_09 = new ol.layer.Tile({
    source: Source_t10m_09
});
Layer_t10m_09.setOpacity(0.75);
Layer_t10m_09.setVisible(false);

// источник данных и слой t10m_10
var Source_t10m_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_10 = new ol.layer.Tile({
    source: Source_t10m_10
});
Layer_t10m_10.setOpacity(0.75);
Layer_t10m_10.setVisible(false);

// источник данных и слой t10m_11
var Source_t10m_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_11 = new ol.layer.Tile({
    source: Source_t10m_11
});
Layer_t10m_11.setOpacity(0.75);
Layer_t10m_11.setVisible(false);

// источник данных и слой t10m_12
var Source_t10m_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:t10m_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_12 = new ol.layer.Tile({
    source: Source_t10m_12
});
Layer_t10m_12.setOpacity(0.75);
Layer_t10m_12.setVisible(false);

// источник данных и слой p_clr_cky_year
var Source_p_clr_cky_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_year = new ol.layer.Tile({
    source: Source_p_clr_cky_year
});
Layer_p_clr_cky_year.setOpacity(0.75);
Layer_p_clr_cky_year.setVisible(false);

// источник данных и слой p_clr_cky_01
var Source_p_clr_cky_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_01 = new ol.layer.Tile({
    source: Source_p_clr_cky_01
});
Layer_p_clr_cky_01.setOpacity(0.75);
Layer_p_clr_cky_01.setVisible(false);

// источник данных и слой p_clr_cky_02
var Source_p_clr_cky_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_02 = new ol.layer.Tile({
    source: Source_p_clr_cky_02
});
Layer_p_clr_cky_02.setOpacity(0.75);
Layer_p_clr_cky_02.setVisible(false);

// источник данных и слой p_clr_cky_03
var Source_p_clr_cky_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_03 = new ol.layer.Tile({
    source: Source_p_clr_cky_03
});
Layer_p_clr_cky_03.setOpacity(0.75);
Layer_p_clr_cky_03.setVisible(false);

// источник данных и слой p_clr_cky_04
var Source_p_clr_cky_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_04 = new ol.layer.Tile({
    source: Source_p_clr_cky_04
});
Layer_p_clr_cky_04.setOpacity(0.75);
Layer_p_clr_cky_04.setVisible(false);

// источник данных и слой p_clr_cky_05
var Source_p_clr_cky_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_05 = new ol.layer.Tile({
    source: Source_p_clr_cky_05
});
Layer_p_clr_cky_05.setOpacity(0.75);
Layer_p_clr_cky_05.setVisible(false);

// источник данных и слой p_clr_cky_06
var Source_p_clr_cky_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_06 = new ol.layer.Tile({
    source: Source_p_clr_cky_06
});
Layer_p_clr_cky_06.setOpacity(0.75);
Layer_p_clr_cky_06.setVisible(false);

// источник данных и слой p_clr_cky_07
var Source_p_clr_cky_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_07 = new ol.layer.Tile({
    source: Source_p_clr_cky_07
});
Layer_p_clr_cky_07.setOpacity(0.75);
Layer_p_clr_cky_07.setVisible(false);

// источник данных и слой p_clr_cky_08
var Source_p_clr_cky_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_08 = new ol.layer.Tile({
    source: Source_p_clr_cky_08
});
Layer_p_clr_cky_08.setOpacity(0.75);
Layer_p_clr_cky_08.setVisible(false);

// источник данных и слой p_clr_cky_09
var Source_p_clr_cky_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_09 = new ol.layer.Tile({
    source: Source_p_clr_cky_09
});
Layer_p_clr_cky_09.setOpacity(0.75);
Layer_p_clr_cky_09.setVisible(false);

// источник данных и слой p_clr_cky_10
var Source_p_clr_cky_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_10 = new ol.layer.Tile({
    source: Source_p_clr_cky_10
});
Layer_p_clr_cky_10.setOpacity(0.75);
Layer_p_clr_cky_10.setVisible(false);

// источник данных и слой p_clr_cky_11
var Source_p_clr_cky_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_11 = new ol.layer.Tile({
    source: Source_p_clr_cky_11
});
Layer_p_clr_cky_11.setOpacity(0.75);
Layer_p_clr_cky_11.setVisible(false);

// источник данных и слой p_clr_cky_12
var Source_p_clr_cky_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_clr_cky_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_clr_cky_12 = new ol.layer.Tile({
    source: Source_p_clr_cky_12
});
Layer_p_clr_cky_12.setOpacity(0.75);
Layer_p_clr_cky_12.setVisible(false);

// источник данных и слой p_swv_dwn_year
var Source_p_swv_dwn_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_year = new ol.layer.Tile({
    source: Source_p_swv_dwn_year
});
Layer_p_swv_dwn_year.setOpacity(0.75);
Layer_p_swv_dwn_year.setVisible(false);

// источник данных и слой p_swv_dwn_01
var Source_p_swv_dwn_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_01 = new ol.layer.Tile({
    source: Source_p_swv_dwn_01
});
Layer_p_swv_dwn_01.setOpacity(0.75);
Layer_p_swv_dwn_01.setVisible(false);

// источник данных и слой p_swv_dwn_02
var Source_p_swv_dwn_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_02 = new ol.layer.Tile({
    source: Source_p_swv_dwn_02
});
Layer_p_swv_dwn_02.setOpacity(0.75);
Layer_p_swv_dwn_02.setVisible(false);

// источник данных и слой p_swv_dwn_03
var Source_p_swv_dwn_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_03 = new ol.layer.Tile({
    source: Source_p_swv_dwn_03
});
Layer_p_swv_dwn_03.setOpacity(0.75);
Layer_p_swv_dwn_03.setVisible(false);

// источник данных и слой p_swv_dwn_04
var Source_p_swv_dwn_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_04 = new ol.layer.Tile({
    source: Source_p_swv_dwn_04
});
Layer_p_swv_dwn_04.setOpacity(0.75);
Layer_p_swv_dwn_04.setVisible(false);

// источник данных и слой p_swv_dwn_05
var Source_p_swv_dwn_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_05 = new ol.layer.Tile({
    source: Source_p_swv_dwn_05
});
Layer_p_swv_dwn_05.setOpacity(0.75);
Layer_p_swv_dwn_05.setVisible(false);

// источник данных и слой p_swv_dwn_06
var Source_p_swv_dwn_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_06 = new ol.layer.Tile({
    source: Source_p_swv_dwn_06
});
Layer_p_swv_dwn_06.setOpacity(0.75);
Layer_p_swv_dwn_06.setVisible(false);

// источник данных и слой p_swv_dwn_07
var Source_p_swv_dwn_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_07 = new ol.layer.Tile({
    source: Source_p_swv_dwn_07
});
Layer_p_swv_dwn_07.setOpacity(0.75);
Layer_p_swv_dwn_07.setVisible(false);

// источник данных и слой p_swv_dwn_08
var Source_p_swv_dwn_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_08 = new ol.layer.Tile({
    source: Source_p_swv_dwn_08
});
Layer_p_swv_dwn_08.setOpacity(0.75);
Layer_p_swv_dwn_08.setVisible(false);

// источник данных и слой p_swv_dwn_09
var Source_p_swv_dwn_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_09 = new ol.layer.Tile({
    source: Source_p_swv_dwn_09
});
Layer_p_swv_dwn_09.setOpacity(0.75);
Layer_p_swv_dwn_09.setVisible(false);

// источник данных и слой p_swv_dwn_10
var Source_p_swv_dwn_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_10 = new ol.layer.Tile({
    source: Source_p_swv_dwn_10
});
Layer_p_swv_dwn_10.setOpacity(0.75);
Layer_p_swv_dwn_10.setVisible(false);

// источник данных и слой p_swv_dwn_11
var Source_p_swv_dwn_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_11 = new ol.layer.Tile({
    source: Source_p_swv_dwn_11
});
Layer_p_swv_dwn_11.setOpacity(0.75);
Layer_p_swv_dwn_11.setVisible(false);

// источник данных и слой p_swv_dwn_12
var Source_p_swv_dwn_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_swv_dwn_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_swv_dwn_12 = new ol.layer.Tile({
    source: Source_p_swv_dwn_12
});
Layer_p_swv_dwn_12.setOpacity(0.75);
Layer_p_swv_dwn_12.setVisible(false);

// источник данных и слой p_toa_dwn_year
var Source_p_toa_dwn_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_year',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_year = new ol.layer.Tile({
    source: Source_p_toa_dwn_year
});
Layer_p_toa_dwn_year.setOpacity(0.75);
Layer_p_toa_dwn_year.setVisible(false);

// источник данных и слой p_toa_dwn_01
var Source_p_toa_dwn_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_01',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_01 = new ol.layer.Tile({
    source: Source_p_toa_dwn_01
});
Layer_p_toa_dwn_01.setOpacity(0.75);
Layer_p_toa_dwn_01.setVisible(false);

// источник данных и слой p_toa_dwn_02
var Source_p_toa_dwn_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_02',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_02 = new ol.layer.Tile({
    source: Source_p_toa_dwn_02
});
Layer_p_toa_dwn_02.setOpacity(0.75);
Layer_p_toa_dwn_02.setVisible(false);

// источник данных и слой p_toa_dwn_03
var Source_p_toa_dwn_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_03',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_03 = new ol.layer.Tile({
    source: Source_p_toa_dwn_03
});
Layer_p_toa_dwn_03.setOpacity(0.75);
Layer_p_toa_dwn_03.setVisible(false);

// источник данных и слой p_toa_dwn_04
var Source_p_toa_dwn_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_04',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_04 = new ol.layer.Tile({
    source: Source_p_toa_dwn_04
});
Layer_p_toa_dwn_04.setOpacity(0.75);
Layer_p_toa_dwn_04.setVisible(false);

// источник данных и слой p_toa_dwn_05
var Source_p_toa_dwn_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_05',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_05 = new ol.layer.Tile({
    source: Source_p_toa_dwn_05
});
Layer_p_toa_dwn_05.setOpacity(0.75);
Layer_p_toa_dwn_05.setVisible(false);

// источник данных и слой p_toa_dwn_06
var Source_p_toa_dwn_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_06',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_06 = new ol.layer.Tile({
    source: Source_p_toa_dwn_06
});
Layer_p_toa_dwn_06.setOpacity(0.75);
Layer_p_toa_dwn_06.setVisible(false);

// источник данных и слой p_toa_dwn_07
var Source_p_toa_dwn_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_07',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_07 = new ol.layer.Tile({
    source: Source_p_toa_dwn_07
});
Layer_p_toa_dwn_07.setOpacity(0.75);
Layer_p_toa_dwn_07.setVisible(false);

// источник данных и слой p_toa_dwn_08
var Source_p_toa_dwn_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_08',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_08 = new ol.layer.Tile({
    source: Source_p_toa_dwn_08
});
Layer_p_toa_dwn_08.setOpacity(0.75);
Layer_p_toa_dwn_08.setVisible(false);

// источник данных и слой p_toa_dwn_09
var Source_p_toa_dwn_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_09',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_09 = new ol.layer.Tile({
    source: Source_p_toa_dwn_09
});
Layer_p_toa_dwn_09.setOpacity(0.75);
Layer_p_toa_dwn_09.setVisible(false);

// источник данных и слой p_toa_dwn_10
var Source_p_toa_dwn_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_10',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_10 = new ol.layer.Tile({
    source: Source_p_toa_dwn_10
});
Layer_p_toa_dwn_10.setOpacity(0.75);
Layer_p_toa_dwn_10.setVisible(false);

// источник данных и слой p_toa_dwn_11
var Source_p_toa_dwn_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_11',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_11 = new ol.layer.Tile({
    source: Source_p_toa_dwn_11
});
Layer_p_toa_dwn_11.setOpacity(0.75);
Layer_p_toa_dwn_11.setVisible(false);

// источник данных и слой p_toa_dwn_12
var Source_p_toa_dwn_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:p_toa_dwn_12',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_p_toa_dwn_12 = new ol.layer.Tile({
    source: Source_p_toa_dwn_12
});
Layer_p_toa_dwn_12.setOpacity(0.75);
Layer_p_toa_dwn_12.setVisible(false);

// источник данных и слой kzcover
var Source_kzcover = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:kzcover',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_kzcover = new ol.layer.Tile({
    source: Source_kzcover
});
Layer_kzcover.setOpacity(0.75);
Layer_kzcover.setVisible(false);

// источник данных и слой rettlt0opt_year
var Source_rettlt0opt_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:yr-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_year = new ol.layer.Tile({
    source: Source_rettlt0opt_year
});
Layer_rettlt0opt_year.setOpacity(0.75);
Layer_rettlt0opt_year.setVisible(false);

// источник данных и слой rettlt0opt_01
var Source_rettlt0opt_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_01 = new ol.layer.Tile({
    source: Source_rettlt0opt_01
});
Layer_rettlt0opt_01.setOpacity(0.75);
Layer_rettlt0opt_01.setVisible(false);

// источник данных и слой rettlt0opt_02
var Source_rettlt0opt_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_02 = new ol.layer.Tile({
    source: Source_rettlt0opt_02
});
Layer_rettlt0opt_02.setOpacity(0.75);
Layer_rettlt0opt_02.setVisible(false);

// источник данных и слой rettlt0opt_03
var Source_rettlt0opt_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_03 = new ol.layer.Tile({
    source: Source_rettlt0opt_03
});
Layer_rettlt0opt_03.setOpacity(0.75);
Layer_rettlt0opt_03.setVisible(false);

// источник данных и слой rettlt0opt_04
var Source_rettlt0opt_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_04 = new ol.layer.Tile({
    source: Source_rettlt0opt_04
});
Layer_rettlt0opt_04.setOpacity(0.75);
Layer_rettlt0opt_04.setVisible(false);

// источник данных и слой rettlt0opt_05
var Source_rettlt0opt_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_05 = new ol.layer.Tile({
    source: Source_rettlt0opt_05
});
Layer_rettlt0opt_05.setOpacity(0.75);
Layer_rettlt0opt_05.setVisible(false);

// источник данных и слой rettlt0opt_06
var Source_rettlt0opt_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_06 = new ol.layer.Tile({
    source: Source_rettlt0opt_06
});
Layer_rettlt0opt_06.setOpacity(0.75);
Layer_rettlt0opt_06.setVisible(false);

// источник данных и слой rettlt0opt_07
var Source_rettlt0opt_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_07 = new ol.layer.Tile({
    source: Source_rettlt0opt_07
});
Layer_rettlt0opt_07.setOpacity(0.75);
Layer_rettlt0opt_07.setVisible(false);

// источник данных и слой rettlt0opt_08
var Source_rettlt0opt_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_08 = new ol.layer.Tile({
    source: Source_rettlt0opt_08
});
Layer_rettlt0opt_08.setOpacity(0.75);
Layer_rettlt0opt_08.setVisible(false);

// источник данных и слой rettlt0opt_09
var Source_rettlt0opt_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_09 = new ol.layer.Tile({
    source: Source_rettlt0opt_09
});
Layer_rettlt0opt_09.setOpacity(0.75);
Layer_rettlt0opt_09.setVisible(false);

// источник данных и слой rettlt0opt_10
var Source_rettlt0opt_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_10 = new ol.layer.Tile({
    source: Source_rettlt0opt_10
});
Layer_rettlt0opt_10.setOpacity(0.75);
Layer_rettlt0opt_10.setVisible(false);

// источник данных и слой rettlt0opt_11
var Source_rettlt0opt_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_11 = new ol.layer.Tile({
    source: Source_rettlt0opt_11
});
Layer_rettlt0opt_11.setOpacity(0.75);
Layer_rettlt0opt_11.setVisible(false);

// источник данных и слой rettlt0opt_12
var Source_rettlt0opt_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12-rettlt0opt',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rettlt0opt_12 = new ol.layer.Tile({
    source: Source_rettlt0opt_12
});
Layer_rettlt0opt_12.setOpacity(0.75);
Layer_rettlt0opt_12.setVisible(false);

// источник данных и слой clrskyavrg_year
var Source_clrskyavrg_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:yr_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_year = new ol.layer.Tile({
    source: Source_clrskyavrg_year
});
Layer_clrskyavrg_year.setOpacity(0.75);
Layer_clrskyavrg_year.setVisible(false);

// источник данных и слой clrskyavrg_01
var Source_clrskyavrg_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_01 = new ol.layer.Tile({
    source: Source_clrskyavrg_01
});
Layer_clrskyavrg_01.setOpacity(0.75);
Layer_clrskyavrg_01.setVisible(false);

// источник данных и слой clrskyavrg_02
var Source_clrskyavrg_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_02 = new ol.layer.Tile({
    source: Source_clrskyavrg_02
});
Layer_clrskyavrg_02.setOpacity(0.75);
Layer_clrskyavrg_02.setVisible(false);

// источник данных и слой clrskyavrg_03
var Source_clrskyavrg_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_03 = new ol.layer.Tile({
    source: Source_clrskyavrg_03
});
Layer_clrskyavrg_03.setOpacity(0.75);
Layer_clrskyavrg_03.setVisible(false);

// источник данных и слой clrskyavrg_04
var Source_clrskyavrg_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_04 = new ol.layer.Tile({
    source: Source_clrskyavrg_04
});
Layer_clrskyavrg_04.setOpacity(0.75);
Layer_clrskyavrg_04.setVisible(false);

// источник данных и слой clrskyavrg_05
var Source_clrskyavrg_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_05 = new ol.layer.Tile({
    source: Source_clrskyavrg_05
});
Layer_clrskyavrg_05.setOpacity(0.75);
Layer_clrskyavrg_05.setVisible(false);

// источник данных и слой clrskyavrg_06
var Source_clrskyavrg_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_06 = new ol.layer.Tile({
    source: Source_clrskyavrg_06
});
Layer_clrskyavrg_06.setOpacity(0.75);
Layer_clrskyavrg_06.setVisible(false);

// источник данных и слой clrskyavrg_07
var Source_clrskyavrg_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_07 = new ol.layer.Tile({
    source: Source_clrskyavrg_07
});
Layer_clrskyavrg_07.setOpacity(0.75);
Layer_clrskyavrg_07.setVisible(false);

// источник данных и слой clrskyavrg_08
var Source_clrskyavrg_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_08 = new ol.layer.Tile({
    source: Source_clrskyavrg_08
});
Layer_clrskyavrg_08.setOpacity(0.75);
Layer_clrskyavrg_08.setVisible(false);

// источник данных и слой clrskyavrg_09
var Source_clrskyavrg_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_09 = new ol.layer.Tile({
    source: Source_clrskyavrg_09
});
Layer_clrskyavrg_09.setOpacity(0.75);
Layer_clrskyavrg_09.setVisible(false);

// источник данных и слой clrskyavrg_10
var Source_clrskyavrg_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_10 = new ol.layer.Tile({
    source: Source_clrskyavrg_10
});
Layer_clrskyavrg_10.setOpacity(0.75);
Layer_clrskyavrg_10.setVisible(false);

// источник данных и слой clrskyavrg_11
var Source_clrskyavrg_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_11 = new ol.layer.Tile({
    source: Source_clrskyavrg_11
});
Layer_clrskyavrg_11.setOpacity(0.75);
Layer_clrskyavrg_11.setVisible(false);

// источник данных и слой clrskyavrg_12
var Source_clrskyavrg_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_clrskyavrg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_clrskyavrg_12 = new ol.layer.Tile({
    source: Source_clrskyavrg_12
});
Layer_clrskyavrg_12.setOpacity(0.75);
Layer_clrskyavrg_12.setVisible(false);

// источник данных и слой retesh0mim_year
var Source_retesh0mim_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:yr_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_year = new ol.layer.Tile({
    source: Source_retesh0mim_year
});
Layer_retesh0mim_year.setOpacity(0.75);
Layer_retesh0mim_year.setVisible(false);

// источник данных и слой retesh0mim_01
var Source_retesh0mim_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_01 = new ol.layer.Tile({
    source: Source_retesh0mim_01
});
Layer_retesh0mim_01.setOpacity(0.75);
Layer_retesh0mim_01.setVisible(false);

// источник данных и слой retesh0mim_02
var Source_retesh0mim_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_02 = new ol.layer.Tile({
    source: Source_retesh0mim_02
});
Layer_retesh0mim_02.setOpacity(0.75);
Layer_retesh0mim_02.setVisible(false);

// источник данных и слой retesh0mim_03
var Source_retesh0mim_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_03 = new ol.layer.Tile({
    source: Source_retesh0mim_03
});
Layer_retesh0mim_03.setOpacity(0.75);
Layer_retesh0mim_03.setVisible(false);

// источник данных и слой retesh0mim_04
var Source_retesh0mim_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_04 = new ol.layer.Tile({
    source: Source_retesh0mim_04
});
Layer_retesh0mim_04.setOpacity(0.75);
Layer_retesh0mim_04.setVisible(false);

// источник данных и слой retesh0mim_05
var Source_retesh0mim_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_05 = new ol.layer.Tile({
    source: Source_retesh0mim_05
});
Layer_retesh0mim_05.setOpacity(0.75);
Layer_retesh0mim_05.setVisible(false);

// источник данных и слой retesh0mim_06
var Source_retesh0mim_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_06 = new ol.layer.Tile({
    source: Source_retesh0mim_06
});
Layer_retesh0mim_06.setOpacity(0.75);
Layer_retesh0mim_06.setVisible(false);

// источник данных и слой retesh0mim_07
var Source_retesh0mim_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_07 = new ol.layer.Tile({
    source: Source_retesh0mim_07
});
Layer_retesh0mim_07.setOpacity(0.75);
Layer_retesh0mim_07.setVisible(false);

// источник данных и слой retesh0mim_08
var Source_retesh0mim_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_08 = new ol.layer.Tile({
    source: Source_retesh0mim_08
});
Layer_retesh0mim_08.setOpacity(0.75);
Layer_retesh0mim_08.setVisible(false);

// источник данных и слой retesh0mim_09
var Source_retesh0mim_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_09 = new ol.layer.Tile({
    source: Source_retesh0mim_09
});
Layer_retesh0mim_09.setOpacity(0.75);
Layer_retesh0mim_09.setVisible(false);

// источник данных и слой retesh0mim_10
var Source_retesh0mim_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_10 = new ol.layer.Tile({
    source: Source_retesh0mim_10
});
Layer_retesh0mim_10.setOpacity(0.75);
Layer_retesh0mim_10.setVisible(false);

// источник данных и слой retesh0mim_11
var Source_retesh0mim_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_11 = new ol.layer.Tile({
    source: Source_retesh0mim_11
});
Layer_retesh0mim_11.setOpacity(0.75);
Layer_retesh0mim_11.setVisible(false);

// источник данных и слой retesh0mim_12
var Source_retesh0mim_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_retesh0mim',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_retesh0mim_12 = new ol.layer.Tile({
    source: Source_retesh0mim_12
});
Layer_retesh0mim_12.setOpacity(0.75);
Layer_retesh0mim_12.setVisible(false);

// источник данных и слой rainavgesm_year
var Source_rainavgesm_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:yr_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_year = new ol.layer.Tile({
    source: Source_rainavgesm_year
});
Layer_rainavgesm_year.setOpacity(0.75);
Layer_rainavgesm_year.setVisible(false);

// источник данных и слой rainavgesm_01
var Source_rainavgesm_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_01 = new ol.layer.Tile({
    source: Source_rainavgesm_01
});
Layer_rainavgesm_01.setOpacity(0.75);
Layer_rainavgesm_01.setVisible(false);

// источник данных и слой rainavgesm_02
var Source_rainavgesm_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_02 = new ol.layer.Tile({
    source: Source_rainavgesm_02
});
Layer_rainavgesm_02.setOpacity(0.75);
Layer_rainavgesm_02.setVisible(false);

// источник данных и слой rainavgesm_03
var Source_rainavgesm_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_03 = new ol.layer.Tile({
    source: Source_rainavgesm_03
});
Layer_rainavgesm_03.setOpacity(0.75);
Layer_rainavgesm_03.setVisible(false);

// источник данных и слой rainavgesm_04
var Source_rainavgesm_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_04 = new ol.layer.Tile({
    source: Source_rainavgesm_04
});
Layer_rainavgesm_04.setOpacity(0.75);
Layer_rainavgesm_04.setVisible(false);

// источник данных и слой rainavgesm_05
var Source_rainavgesm_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_05 = new ol.layer.Tile({
    source: Source_rainavgesm_05
});
Layer_rainavgesm_05.setOpacity(0.75);
Layer_rainavgesm_05.setVisible(false);

// источник данных и слой rainavgesm_06
var Source_rainavgesm_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_06 = new ol.layer.Tile({
    source: Source_rainavgesm_06
});
Layer_rainavgesm_06.setOpacity(0.75);
Layer_rainavgesm_06.setVisible(false);

// источник данных и слой rainavgesm_07
var Source_rainavgesm_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_07 = new ol.layer.Tile({
    source: Source_rainavgesm_07
});
Layer_rainavgesm_07.setOpacity(0.75);
Layer_rainavgesm_07.setVisible(false);

// источник данных и слой rainavgesm_08
var Source_rainavgesm_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_08 = new ol.layer.Tile({
    source: Source_rainavgesm_08
});
Layer_rainavgesm_08.setOpacity(0.75);
Layer_rainavgesm_08.setVisible(false);

// источник данных и слой rainavgesm_09
var Source_rainavgesm_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_09 = new ol.layer.Tile({
    source: Source_rainavgesm_09
});
Layer_rainavgesm_09.setOpacity(0.75);
Layer_rainavgesm_09.setVisible(false);

// источник данных и слой rainavgesm_10
var Source_rainavgesm_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_10 = new ol.layer.Tile({
    source: Source_rainavgesm_10
});
Layer_rainavgesm_10.setOpacity(0.75);
Layer_rainavgesm_10.setVisible(false);

// источник данных и слой rainavgesm_11
var Source_rainavgesm_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_11 = new ol.layer.Tile({
    source: Source_rainavgesm_11
});
Layer_rainavgesm_11.setOpacity(0.75);
Layer_rainavgesm_11.setVisible(false);

// источник данных и слой rainavgesm_12
var Source_rainavgesm_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_rainavgesm',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_rainavgesm_12 = new ol.layer.Tile({
    source: Source_rainavgesm_12
});
Layer_rainavgesm_12.setOpacity(0.75);
Layer_rainavgesm_12.setVisible(false);

// источник данных и слой t10mmax_01
var Source_t10mmax_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_01 = new ol.layer.Tile({
    source: Source_t10mmax_01
});
Layer_t10mmax_01.setOpacity(0.75);
Layer_t10mmax_01.setVisible(false);

// источник данных и слой t10mmax_02
var Source_t10mmax_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_02 = new ol.layer.Tile({
    source: Source_t10mmax_02
});
Layer_t10mmax_02.setOpacity(0.75);
Layer_t10mmax_02.setVisible(false);

// источник данных и слой t10mmax_03
var Source_t10mmax_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_03 = new ol.layer.Tile({
    source: Source_t10mmax_03
});
Layer_t10mmax_03.setOpacity(0.75);
Layer_t10mmax_03.setVisible(false);

// источник данных и слой t10mmax_04
var Source_t10mmax_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_04 = new ol.layer.Tile({
    source: Source_t10mmax_04
});
Layer_t10mmax_04.setOpacity(0.75);
Layer_t10mmax_04.setVisible(false);

// источник данных и слой t10mmax_05
var Source_t10mmax_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_05 = new ol.layer.Tile({
    source: Source_t10mmax_05
});
Layer_t10mmax_05.setOpacity(0.75);
Layer_t10mmax_05.setVisible(false);

// источник данных и слой t10mmax_06
var Source_t10mmax_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_06 = new ol.layer.Tile({
    source: Source_t10mmax_06
});
Layer_t10mmax_06.setOpacity(0.75);
Layer_t10mmax_06.setVisible(false);

// источник данных и слой t10mmax_07
var Source_t10mmax_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_07 = new ol.layer.Tile({
    source: Source_t10mmax_07
});
Layer_t10mmax_07.setOpacity(0.75);
Layer_t10mmax_07.setVisible(false);

// источник данных и слой t10mmax_08
var Source_t10mmax_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_08 = new ol.layer.Tile({
    source: Source_t10mmax_08
});
Layer_t10mmax_08.setOpacity(0.75);
Layer_t10mmax_08.setVisible(false);

// источник данных и слой t10mmax_09
var Source_t10mmax_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_09 = new ol.layer.Tile({
    source: Source_t10mmax_09
});
Layer_t10mmax_09.setOpacity(0.75);
Layer_t10mmax_09.setVisible(false);

// источник данных и слой t10mmax_10
var Source_t10mmax_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_10 = new ol.layer.Tile({
    source: Source_t10mmax_10
});
Layer_t10mmax_10.setOpacity(0.75);
Layer_t10mmax_10.setVisible(false);

// источник данных и слой t10mmax_11
var Source_t10mmax_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_11 = new ol.layer.Tile({
    source: Source_t10mmax_11
});
Layer_t10mmax_11.setOpacity(0.75);
Layer_t10mmax_11.setVisible(false);

// источник данных и слой t10mmax_12
var Source_t10mmax_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_t10mmax',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10mmax_12 = new ol.layer.Tile({
    source: Source_t10mmax_12
});
Layer_t10mmax_12.setOpacity(0.75);
Layer_t10mmax_12.setVisible(false);

// источник данных и слой t10m_min_01
var Source_t10m_min_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_01 = new ol.layer.Tile({
    source: Source_t10m_min_01
});
Layer_t10m_min_01.setOpacity(0.75);
Layer_t10m_min_01.setVisible(false);

// источник данных и слой t10m_min_02
var Source_t10m_min_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_02 = new ol.layer.Tile({
    source: Source_t10m_min_02
});
Layer_t10m_min_02.setOpacity(0.75);
Layer_t10m_min_02.setVisible(false);

// источник данных и слой t10m_min_03
var Source_t10m_min_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_03 = new ol.layer.Tile({
    source: Source_t10m_min_03
});
Layer_t10m_min_03.setOpacity(0.75);
Layer_t10m_min_03.setVisible(false);

// источник данных и слой t10m_min_04
var Source_t10m_min_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_04 = new ol.layer.Tile({
    source: Source_t10m_min_04
});
Layer_t10m_min_04.setOpacity(0.75);
Layer_t10m_min_04.setVisible(false);

// источник данных и слой t10m_min_05
var Source_t10m_min_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_05 = new ol.layer.Tile({
    source: Source_t10m_min_05
});
Layer_t10m_min_05.setOpacity(0.75);
Layer_t10m_min_05.setVisible(false);

// источник данных и слой t10m_min_06
var Source_t10m_min_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_06 = new ol.layer.Tile({
    source: Source_t10m_min_06
});
Layer_t10m_min_06.setOpacity(0.75);
Layer_t10m_min_06.setVisible(false);

// источник данных и слой t10m_min_07
var Source_t10m_min_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_07 = new ol.layer.Tile({
    source: Source_t10m_min_07
});
Layer_t10m_min_07.setOpacity(0.75);
Layer_t10m_min_07.setVisible(false);

// источник данных и слой t10m_min_08
var Source_t10m_min_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_08 = new ol.layer.Tile({
    source: Source_t10m_min_08
});
Layer_t10m_min_08.setOpacity(0.75);
Layer_t10m_min_08.setVisible(false);

// источник данных и слой t10m_min_09
var Source_t10m_min_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_09 = new ol.layer.Tile({
    source: Source_t10m_min_09
});
Layer_t10m_min_09.setOpacity(0.75);
Layer_t10m_min_09.setVisible(false);

// источник данных и слой t10m_min_10
var Source_t10m_min_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_10 = new ol.layer.Tile({
    source: Source_t10m_min_10
});
Layer_t10m_min_10.setOpacity(0.75);
Layer_t10m_min_10.setVisible(false);

// источник данных и слой t10m_min_11
var Source_t10m_min_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_11 = new ol.layer.Tile({
    source: Source_t10m_min_11
});
Layer_t10m_min_11.setOpacity(0.75);
Layer_t10m_min_11.setVisible(false);

// источник данных и слой t10m_min_12
var Source_t10m_min_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_t10m_min',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_t10m_min_12 = new ol.layer.Tile({
    source: Source_t10m_min_12
});
Layer_t10m_min_12.setOpacity(0.75);
Layer_t10m_min_12.setVisible(false);

// источник данных и слой tskinavg_01
var Source_tskinavg_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_01 = new ol.layer.Tile({
    source: Source_tskinavg_01
});
Layer_tskinavg_01.setOpacity(0.75);
Layer_tskinavg_01.setVisible(false);

// источник данных и слой tskinavg_02
var Source_tskinavg_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_02 = new ol.layer.Tile({
    source: Source_tskinavg_02
});
Layer_tskinavg_02.setOpacity(0.75);
Layer_tskinavg_02.setVisible(false);

// источник данных и слой tskinavg_03
var Source_tskinavg_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_03 = new ol.layer.Tile({
    source: Source_tskinavg_03
});
Layer_tskinavg_03.setOpacity(0.75);
Layer_tskinavg_03.setVisible(false);

// источник данных и слой tskinavg_04
var Source_tskinavg_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_04 = new ol.layer.Tile({
    source: Source_tskinavg_04
});
Layer_tskinavg_04.setOpacity(0.75);
Layer_tskinavg_04.setVisible(false);

// источник данных и слой tskinavg_05
var Source_tskinavg_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_05 = new ol.layer.Tile({
    source: Source_tskinavg_05
});
Layer_tskinavg_05.setOpacity(0.75);
Layer_tskinavg_05.setVisible(false);

// источник данных и слой tskinavg_06
var Source_tskinavg_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_06 = new ol.layer.Tile({
    source: Source_tskinavg_06
});
Layer_tskinavg_06.setOpacity(0.75);
Layer_tskinavg_06.setVisible(false);

// источник данных и слой tskinavg_07
var Source_tskinavg_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_07 = new ol.layer.Tile({
    source: Source_tskinavg_07
});
Layer_tskinavg_07.setOpacity(0.75);
Layer_tskinavg_07.setVisible(false);

// источник данных и слой tskinavg_08
var Source_tskinavg_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_08 = new ol.layer.Tile({
    source: Source_tskinavg_08
});
Layer_tskinavg_08.setOpacity(0.75);
Layer_tskinavg_08.setVisible(false);

// источник данных и слой tskinavg_09
var Source_tskinavg_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_09 = new ol.layer.Tile({
    source: Source_tskinavg_09
});
Layer_tskinavg_09.setOpacity(0.75);
Layer_tskinavg_09.setVisible(false);

// источник данных и слой tskinavg_10
var Source_tskinavg_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_10 = new ol.layer.Tile({
    source: Source_tskinavg_10
});
Layer_tskinavg_10.setOpacity(0.75);
Layer_tskinavg_10.setVisible(false);

// источник данных и слой tskinavg_11
var Source_tskinavg_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_11 = new ol.layer.Tile({
    source: Source_tskinavg_11
});
Layer_tskinavg_11.setOpacity(0.75);
Layer_tskinavg_11.setVisible(false);

// источник данных и слой tskinavg_12
var Source_tskinavg_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_tskinavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_tskinavg_12 = new ol.layer.Tile({
    source: Source_tskinavg_12
});
Layer_tskinavg_12.setOpacity(0.75);
Layer_tskinavg_12.setVisible(false);

// источник данных и слой srfalbavg_01
var Source_srfalbavg_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_01 = new ol.layer.Tile({
    source: Source_srfalbavg_01
});
Layer_srfalbavg_01.setOpacity(0.75);
Layer_srfalbavg_01.setVisible(false);

// источник данных и слой srfalbavg_02
var Source_srfalbavg_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_02 = new ol.layer.Tile({
    source: Source_srfalbavg_02
});
Layer_srfalbavg_02.setOpacity(0.75);
Layer_srfalbavg_02.setVisible(false);

// источник данных и слой srfalbavg_03
var Source_srfalbavg_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_03 = new ol.layer.Tile({
    source: Source_srfalbavg_03
});
Layer_srfalbavg_03.setOpacity(0.75);
Layer_srfalbavg_03.setVisible(false);

// источник данных и слой srfalbavg_04
var Source_srfalbavg_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_04 = new ol.layer.Tile({
    source: Source_srfalbavg_04
});
Layer_srfalbavg_04.setOpacity(0.75);
Layer_srfalbavg_04.setVisible(false);

// источник данных и слой srfalbavg_05
var Source_srfalbavg_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_05 = new ol.layer.Tile({
    source: Source_srfalbavg_05
});
Layer_srfalbavg_05.setOpacity(0.75);
Layer_srfalbavg_05.setVisible(false);

// источник данных и слой srfalbavg_06
var Source_srfalbavg_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_06 = new ol.layer.Tile({
    source: Source_srfalbavg_06
});
Layer_srfalbavg_06.setOpacity(0.75);
Layer_srfalbavg_06.setVisible(false);

// источник данных и слой srfalbavg_07
var Source_srfalbavg_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_07 = new ol.layer.Tile({
    source: Source_srfalbavg_07
});
Layer_srfalbavg_07.setOpacity(0.75);
Layer_srfalbavg_07.setVisible(false);

// источник данных и слой srfalbavg_08
var Source_srfalbavg_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_08 = new ol.layer.Tile({
    source: Source_srfalbavg_08
});
Layer_srfalbavg_08.setOpacity(0.75);
Layer_srfalbavg_08.setVisible(false);

// источник данных и слой srfalbavg_09
var Source_srfalbavg_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_09 = new ol.layer.Tile({
    source: Source_srfalbavg_09
});
Layer_srfalbavg_09.setOpacity(0.75);
Layer_srfalbavg_09.setVisible(false);

// источник данных и слой srfalbavg_10
var Source_srfalbavg_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_10 = new ol.layer.Tile({
    source: Source_srfalbavg_10
});
Layer_srfalbavg_10.setOpacity(0.75);
Layer_srfalbavg_10.setVisible(false);

// источник данных и слой srfalbavg_11
var Source_srfalbavg_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_11 = new ol.layer.Tile({
    source: Source_srfalbavg_11
});
Layer_srfalbavg_11.setOpacity(0.75);
Layer_srfalbavg_11.setVisible(false);

// источник данных и слой srfalbavg_12
var Source_srfalbavg_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12_srfalbavg',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_srfalbavg_12 = new ol.layer.Tile({
    source: Source_srfalbavg_12
});
Layer_srfalbavg_12.setOpacity(0.75);
Layer_srfalbavg_12.setVisible(false);

// источник данных и слой sis_klr_year
var Source_sis_klr_year = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:yr-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_year = new ol.layer.Tile({
    source: Source_sis_klr_year
});
Layer_sis_klr_year.setOpacity(0.75);
Layer_sis_klr_year.setVisible(false);

// источник данных и слой sis_klr_01
var Source_sis_klr_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_01 = new ol.layer.Tile({
    source: Source_sis_klr_01
});
Layer_sis_klr_01.setOpacity(0.75);
Layer_sis_klr_01.setVisible(false);

// источник данных и слой sis_klr_02
var Source_sis_klr_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_02 = new ol.layer.Tile({
    source: Source_sis_klr_02
});
Layer_sis_klr_02.setOpacity(0.75);
Layer_sis_klr_02.setVisible(false);

// источник данных и слой sis_klr_03
var Source_sis_klr_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_03 = new ol.layer.Tile({
    source: Source_sis_klr_03
});
Layer_sis_klr_03.setOpacity(0.75);
Layer_sis_klr_03.setVisible(false);

// источник данных и слой sis_klr_04
var Source_sis_klr_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_04 = new ol.layer.Tile({
    source: Source_sis_klr_04
});
Layer_sis_klr_04.setOpacity(0.75);
Layer_sis_klr_04.setVisible(false);

// источник данных и слой sis_klr_05
var Source_sis_klr_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_05 = new ol.layer.Tile({
    source: Source_sis_klr_05
});
Layer_sis_klr_05.setOpacity(0.75);
Layer_sis_klr_05.setVisible(false);

// источник данных и слой sis_klr_06
var Source_sis_klr_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_06 = new ol.layer.Tile({
    source: Source_sis_klr_06
});
Layer_sis_klr_06.setOpacity(0.75);
Layer_sis_klr_06.setVisible(false);

// источник данных и слой sis_klr_07
var Source_sis_klr_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_07 = new ol.layer.Tile({
    source: Source_sis_klr_07
});
Layer_sis_klr_07.setOpacity(0.75);
Layer_sis_klr_07.setVisible(false);

// источник данных и слой sis_klr_08
var Source_sis_klr_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_08 = new ol.layer.Tile({
    source: Source_sis_klr_08
});
Layer_sis_klr_08.setOpacity(0.75);
Layer_sis_klr_08.setVisible(false);

// источник данных и слой sis_klr_09
var Source_sis_klr_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_09 = new ol.layer.Tile({
    source: Source_sis_klr_09
});
Layer_sis_klr_09.setOpacity(0.75);
Layer_sis_klr_09.setVisible(false);

// источник данных и слой sis_klr_10
var Source_sis_klr_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_10 = new ol.layer.Tile({
    source: Source_sis_klr_10
});
Layer_sis_klr_10.setOpacity(0.75);
Layer_sis_klr_10.setVisible(false);

// источник данных и слой sis_klr_11
var Source_sis_klr_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_11 = new ol.layer.Tile({
    source: Source_sis_klr_11
});
Layer_sis_klr_11.setOpacity(0.75);
Layer_sis_klr_11.setVisible(false);

// источник данных и слой sis_klr_12
var Source_sis_klr_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12-sis_klr',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_sis_klr_12 = new ol.layer.Tile({
    source: Source_sis_klr_12
});
Layer_sis_klr_12.setOpacity(0.75);
Layer_sis_klr_12.setVisible(false);

// источник данных и слой avg_kt_22_01
var Source_avg_kt_22_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_01 = new ol.layer.Tile({
    source: Source_avg_kt_22_01
});
Layer_avg_kt_22_01.setOpacity(0.75);
Layer_avg_kt_22_01.setVisible(false);

// источник данных и слой avg_kt_22_02
var Source_avg_kt_22_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_02 = new ol.layer.Tile({
    source: Source_avg_kt_22_02
});
Layer_avg_kt_22_02.setOpacity(0.75);
Layer_avg_kt_22_02.setVisible(false);

// источник данных и слой avg_kt_22_03
var Source_avg_kt_22_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_03 = new ol.layer.Tile({
    source: Source_avg_kt_22_03
});
Layer_avg_kt_22_03.setOpacity(0.75);
Layer_avg_kt_22_03.setVisible(false);

// источник данных и слой avg_kt_22_04
var Source_avg_kt_22_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_04 = new ol.layer.Tile({
    source: Source_avg_kt_22_04
});
Layer_avg_kt_22_04.setOpacity(0.75);
Layer_avg_kt_22_04.setVisible(false);

// источник данных и слой avg_kt_22_05
var Source_avg_kt_22_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_05 = new ol.layer.Tile({
    source: Source_avg_kt_22_05
});
Layer_avg_kt_22_05.setOpacity(0.75);
Layer_avg_kt_22_05.setVisible(false);

// источник данных и слой avg_kt_22_06
var Source_avg_kt_22_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_06 = new ol.layer.Tile({
    source: Source_avg_kt_22_06
});
Layer_avg_kt_22_06.setOpacity(0.75);
Layer_avg_kt_22_06.setVisible(false);

// источник данных и слой avg_kt_22_07
var Source_avg_kt_22_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_07 = new ol.layer.Tile({
    source: Source_avg_kt_22_07
});
Layer_avg_kt_22_07.setOpacity(0.75);
Layer_avg_kt_22_07.setVisible(false);

// источник данных и слой avg_kt_22_08
var Source_avg_kt_22_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_08 = new ol.layer.Tile({
    source: Source_avg_kt_22_08
});
Layer_avg_kt_22_08.setOpacity(0.75);
Layer_avg_kt_22_08.setVisible(false);

// источник данных и слой avg_kt_22_09
var Source_avg_kt_22_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_09 = new ol.layer.Tile({
    source: Source_avg_kt_22_09
});
Layer_avg_kt_22_09.setOpacity(0.75);
Layer_avg_kt_22_09.setVisible(false);

// источник данных и слой avg_kt_22_10
var Source_avg_kt_22_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_10 = new ol.layer.Tile({
    source: Source_avg_kt_22_10
});
Layer_avg_kt_22_10.setOpacity(0.75);
Layer_avg_kt_22_10.setVisible(false);

// источник данных и слой avg_kt_22_11
var Source_avg_kt_22_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_11 = new ol.layer.Tile({
    source: Source_avg_kt_22_11
});
Layer_avg_kt_22_11.setOpacity(0.75);
Layer_avg_kt_22_11.setVisible(false);

// источник данных и слой avg_kt_22_12
var Source_avg_kt_22_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12avg_kt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_kt_22_12 = new ol.layer.Tile({
    source: Source_avg_kt_22_12
});
Layer_avg_kt_22_12.setOpacity(0.75);
Layer_avg_kt_22_12.setVisible(false);

// источник данных и слой avg_nkt_22_01
var Source_avg_nkt_22_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_01 = new ol.layer.Tile({
    source: Source_avg_nkt_22_01
});
Layer_avg_nkt_22_01.setOpacity(0.75);
Layer_avg_nkt_22_01.setVisible(false);

// источник данных и слой avg_nkt_22_02
var Source_avg_nkt_22_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_02 = new ol.layer.Tile({
    source: Source_avg_nkt_22_02
});
Layer_avg_nkt_22_02.setOpacity(0.75);
Layer_avg_nkt_22_02.setVisible(false);

// источник данных и слой avg_nkt_22_03
var Source_avg_nkt_22_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_03 = new ol.layer.Tile({
    source: Source_avg_nkt_22_03
});
Layer_avg_nkt_22_03.setOpacity(0.75);
Layer_avg_nkt_22_03.setVisible(false);

// источник данных и слой avg_nkt_22_04
var Source_avg_nkt_22_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_04 = new ol.layer.Tile({
    source: Source_avg_nkt_22_04
});
Layer_avg_nkt_22_04.setOpacity(0.75);
Layer_avg_nkt_22_04.setVisible(false);

// источник данных и слой avg_nkt_22_05
var Source_avg_nkt_22_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_05 = new ol.layer.Tile({
    source: Source_avg_nkt_22_05
});
Layer_avg_nkt_22_05.setOpacity(0.75);
Layer_avg_nkt_22_05.setVisible(false);

// источник данных и слой avg_nkt_22_06
var Source_avg_nkt_22_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_06 = new ol.layer.Tile({
    source: Source_avg_nkt_22_06
});
Layer_avg_nkt_22_06.setOpacity(0.75);
Layer_avg_nkt_22_06.setVisible(false);

// источник данных и слой avg_nkt_22_07
var Source_avg_nkt_22_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_07 = new ol.layer.Tile({
    source: Source_avg_nkt_22_07
});
Layer_avg_nkt_22_07.setOpacity(0.75);
Layer_avg_nkt_22_07.setVisible(false);

// источник данных и слой avg_nkt_22_08
var Source_avg_nkt_22_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_08 = new ol.layer.Tile({
    source: Source_avg_nkt_22_08
});
Layer_avg_nkt_22_08.setOpacity(0.75);
Layer_avg_nkt_22_08.setVisible(false);

// источник данных и слой avg_nkt_22_09
var Source_avg_nkt_22_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_09 = new ol.layer.Tile({
    source: Source_avg_nkt_22_09
});
Layer_avg_nkt_22_09.setOpacity(0.75);
Layer_avg_nkt_22_09.setVisible(false);

// источник данных и слой avg_nkt_22_10
var Source_avg_nkt_22_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_10 = new ol.layer.Tile({
    source: Source_avg_nkt_22_10
});
Layer_avg_nkt_22_10.setOpacity(0.75);
Layer_avg_nkt_22_10.setVisible(false);

// источник данных и слой avg_nkt_22_11
var Source_avg_nkt_22_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_11 = new ol.layer.Tile({
    source: Source_avg_nkt_22_11
});
Layer_avg_nkt_22_11.setOpacity(0.75);
Layer_avg_nkt_22_11.setVisible(false);

// источник данных и слой avg_nkt_22_12
var Source_avg_nkt_22_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12avg_nkt_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_avg_nkt_22_12 = new ol.layer.Tile({
    source: Source_avg_nkt_22_12
});
Layer_avg_nkt_22_12.setOpacity(0.75);
Layer_avg_nkt_22_12.setVisible(false);

// источник данных и слой day_cld_22_01
var Source_day_cld_22_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_01 = new ol.layer.Tile({
    source: Source_day_cld_22_01
});
Layer_day_cld_22_01.setOpacity(0.75);
Layer_day_cld_22_01.setVisible(false);

// источник данных и слой day_cld_22_02
var Source_day_cld_22_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_02 = new ol.layer.Tile({
    source: Source_day_cld_22_02
});
Layer_day_cld_22_02.setOpacity(0.75);
Layer_day_cld_22_02.setVisible(false);

// источник данных и слой day_cld_22_03
var Source_day_cld_22_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_03 = new ol.layer.Tile({
    source: Source_day_cld_22_03
});
Layer_day_cld_22_03.setOpacity(0.75);
Layer_day_cld_22_03.setVisible(false);

// источник данных и слой day_cld_22_04
var Source_day_cld_22_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_04 = new ol.layer.Tile({
    source: Source_day_cld_22_04
});
Layer_day_cld_22_04.setOpacity(0.75);
Layer_day_cld_22_04.setVisible(false);

// источник данных и слой day_cld_22_05
var Source_day_cld_22_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_05 = new ol.layer.Tile({
    source: Source_day_cld_22_05
});
Layer_day_cld_22_05.setOpacity(0.75);
Layer_day_cld_22_05.setVisible(false);

// источник данных и слой day_cld_22_06
var Source_day_cld_22_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_06 = new ol.layer.Tile({
    source: Source_day_cld_22_06
});
Layer_day_cld_22_06.setOpacity(0.75);
Layer_day_cld_22_06.setVisible(false);

// источник данных и слой day_cld_22_07
var Source_day_cld_22_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_07 = new ol.layer.Tile({
    source: Source_day_cld_22_07
});
Layer_day_cld_22_07.setOpacity(0.75);
Layer_day_cld_22_07.setVisible(false);

// источник данных и слой day_cld_22_08
var Source_day_cld_22_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_08 = new ol.layer.Tile({
    source: Source_day_cld_22_08
});
Layer_day_cld_22_08.setOpacity(0.75);
Layer_day_cld_22_08.setVisible(false);

// источник данных и слой day_cld_22_09
var Source_day_cld_22_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_09 = new ol.layer.Tile({
    source: Source_day_cld_22_09
});
Layer_day_cld_22_09.setOpacity(0.75);
Layer_day_cld_22_09.setVisible(false);

// источник данных и слой day_cld_22_10
var Source_day_cld_22_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_10 = new ol.layer.Tile({
    source: Source_day_cld_22_10
});
Layer_day_cld_22_10.setOpacity(0.75);
Layer_day_cld_22_10.setVisible(false);

// источник данных и слой day_cld_22_11
var Source_day_cld_22_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_11 = new ol.layer.Tile({
    source: Source_day_cld_22_11
});
Layer_day_cld_22_11.setOpacity(0.75);
Layer_day_cld_22_11.setVisible(false);

// источник данных и слой day_cld_22_12
var Source_day_cld_22_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12day_cld_22',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_day_cld_22_12 = new ol.layer.Tile({
    source: Source_day_cld_22_12
});
Layer_day_cld_22_12.setOpacity(0.75);
Layer_day_cld_22_12.setVisible(false);

// источник данных и слой daylghtav_01
var Source_daylghtav_01 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:01daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_01 = new ol.layer.Tile({
    source: Source_daylghtav_01
});
Layer_daylghtav_01.setOpacity(0.75);
Layer_daylghtav_01.setVisible(false);

// источник данных и слой daylghtav_02
var Source_daylghtav_02 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:02daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_02 = new ol.layer.Tile({
    source: Source_daylghtav_02
});
Layer_daylghtav_02.setOpacity(0.75);
Layer_daylghtav_02.setVisible(false);

// источник данных и слой daylghtav_03
var Source_daylghtav_03 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:03daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_03 = new ol.layer.Tile({
    source: Source_daylghtav_03
});
Layer_daylghtav_03.setOpacity(0.75);
Layer_daylghtav_03.setVisible(false);

// источник данных и слой daylghtav_04
var Source_daylghtav_04 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:04daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_04 = new ol.layer.Tile({
    source: Source_daylghtav_04
});
Layer_daylghtav_04.setOpacity(0.75);
Layer_daylghtav_04.setVisible(false);

// источник данных и слой daylghtav_05
var Source_daylghtav_05 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:05daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_05 = new ol.layer.Tile({
    source: Source_daylghtav_05
});
Layer_daylghtav_05.setOpacity(0.75);
Layer_daylghtav_05.setVisible(false);

// источник данных и слой daylghtav_06
var Source_daylghtav_06 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:06daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_06 = new ol.layer.Tile({
    source: Source_daylghtav_06
});
Layer_daylghtav_06.setOpacity(0.75);
Layer_daylghtav_06.setVisible(false);

// источник данных и слой daylghtav_07
var Source_daylghtav_07 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:07daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_07 = new ol.layer.Tile({
    source: Source_daylghtav_07
});
Layer_daylghtav_07.setOpacity(0.75);
Layer_daylghtav_07.setVisible(false);

// источник данных и слой daylghtav_08
var Source_daylghtav_08 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:08daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_08 = new ol.layer.Tile({
    source: Source_daylghtav_08
});
Layer_daylghtav_08.setOpacity(0.75);
Layer_daylghtav_08.setVisible(false);

// источник данных и слой daylghtav_09
var Source_daylghtav_09 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:09daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_09 = new ol.layer.Tile({
    source: Source_daylghtav_09
});
Layer_daylghtav_09.setOpacity(0.75);
Layer_daylghtav_09.setVisible(false);

// источник данных и слой daylghtav_10
var Source_daylghtav_10 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:10daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_10 = new ol.layer.Tile({
    source: Source_daylghtav_10
});
Layer_daylghtav_10.setOpacity(0.75);
Layer_daylghtav_10.setVisible(false);

// источник данных и слой daylghtav_11
var Source_daylghtav_11 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:11daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_11 = new ol.layer.Tile({
    source: Source_daylghtav_11
});
Layer_daylghtav_11.setOpacity(0.75);
Layer_daylghtav_11.setVisible(false);

// источник данных и слой daylghtav_12
var Source_daylghtav_12 = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:12daylghtav',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_daylghtav_12 = new ol.layer.Tile({
    source: Source_daylghtav_12
});
Layer_daylghtav_12.setOpacity(0.75);
Layer_daylghtav_12.setVisible(false);

// источник данных и слой meteo_st
var Source_meteo_st = new ol.source.TileWMS({
    url: 'http://' + gip + ':8080/geoserver/AtlasSolar/wms?',
    params: {
        'LAYERS': 'AtlasSolar:meteo_st',
        'VERSION': '1.1.0',
        'FORMAT': 'image/png',
        'TILED': true
    },
    serverType: 'geoserver'
});
var Layer_meteo_st = new ol.layer.Tile({
    source: Source_meteo_st
});
Layer_meteo_st.setOpacity(0.75);
Layer_meteo_st.setVisible(false);