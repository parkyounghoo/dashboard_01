var mapContainer = document.getElementById('map'), // 지도를 표시할 div
    mapOption = {
        center: new kakao.maps.LatLng(36.054837080789724, 127.64352756503625), // 지도의 중심좌표
        level: 13 // 지도의 확대 레벨
    };

var map = new kakao.maps.Map(mapContainer, mapOption),
    customOverlay = new kakao.maps.CustomOverlay({}),
    infowindow = new kakao.maps.InfoWindow({ removable: true });

function setMapType() {
    removeAllChildNods(document.getElementById('map'));

    var mapContainer = document.getElementById('map'), // 지도를 표시할 div
        mapOption = {
            center: new kakao.maps.LatLng(36.054837080789724, 127.64352756503625), // 지도의 중심좌표
            level: 13 // 지도의 확대 레벨
        };

    var map = new kakao.maps.Map(mapContainer, mapOption),
        customOverlay = new kakao.maps.CustomOverlay({}),
        infowindow = new kakao.maps.InfoWindow({ removable: true });

    var moveLatLon = new kakao.maps.LatLng(36.054837080789724, 127.64352756503625);
    map.setLevel(13);
    map.setCenter(moveLatLon);

    for (var i = 0, len = areas2.length; i < len; i++) {
        sidoArea(areas2[i], map);
    }
}

// 지도에 영역데이터를 폴리곤으로 표시합니다
for (var i = 0, len = areas2.length; i < len; i++) {
    sidoArea(areas2[i], map);
}

function removeAllChildNods(el) {
    while (el.hasChildNodes()) {
        el.removeChild(el.lastChild);
    }
}

function sigunguArea(area, map) {
    $.ajax({
        type: "POST",
        url: "WebService.asmx/GetSigunguList",
        data: "{sido : '" + area.sido + "',code : '" + area.name + "',sDate : '2019-09',eDate : '2020-04'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            // 다각형을 생성합니다
            var polygon = new kakao.maps.Polygon({
                map: map, // 다각형을 표시할 지도 객체
                path: area.path,
                strokeWeight: 2,
                strokeColor: '#004c80',
                strokeOpacity: 0.8,
                fillColor: '#fff',
                fillOpacity: 0.3
            });
            // 다각형에 mouseover 이벤트를 등록하고 이벤트가 발생하면 폴리곤의 채움색을 변경합니다
            // 지역명을 표시하는 커스텀오버레이를 지도위에 표시합니다
            kakao.maps.event.addListener(polygon, 'mouseover', function (mouseEvent) {
                var content = "<div class='area' style='width: 130px; height: 120px;' align='center'>" + area.name
                content += "</br> 건수 : " + data.d.cnt + "";
                if (data.d.top1 != "") {
                    content += "</br> TOP 3";
                    content += "</br> 1. " + data.d.top1 + "";
                }
                if (data.d.top2 != "") {
                    content += "</br> 2. " + data.d.top2 + "";
                }
                if (data.d.top3 != "") {
                    content += "</br> 3. " + data.d.top3 + "";
                }
                content += "</div>";

                polygon.setOptions({ fillColor: '#09f' });
                customOverlay.setContent(content);
                customOverlay.setPosition(mouseEvent.latLng);
                customOverlay.setMap(map);
            });

            // 다각형에 mousemove 이벤트를 등록하고 이벤트가 발생하면 커스텀 오버레이의 위치를 변경합니다
            kakao.maps.event.addListener(polygon, 'mousemove', function (mouseEvent) {
                customOverlay.setPosition(mouseEvent.latLng);
            });

            // 다각형에 mouseout 이벤트를 등록하고 이벤트가 발생하면 폴리곤의 채움색을 원래색으로 변경합니다
            // 커스텀 오버레이를 지도에서 제거합니다
            kakao.maps.event.addListener(polygon, 'mouseout', function () {
                polygon.setOptions({ fillColor: '#fff' });
                customOverlay.setMap(null);
            });
        }
    });
}

// 다각형을 생성하고 이벤트를 등록하는 함수입니다
function sidoArea(area, map) {

    // 다각형을 생성합니다
    var polygon = new kakao.maps.Polygon({
        map: map, // 다각형을 표시할 지도 객체
        path: area.path,
        strokeWeight: 2,
        strokeColor: '#004c80',
        strokeOpacity: 0.8,
        fillColor: '#fff',
        fillOpacity: 0.3
    });
    // 다각형에 mouseover 이벤트를 등록하고 이벤트가 발생하면 폴리곤의 채움색을 변경합니다
    // 지역명을 표시하는 커스텀오버레이를 지도위에 표시합니다
    kakao.maps.event.addListener(polygon, 'mouseover', function (mouseEvent) {
        customOverlay.setPosition(mouseEvent.latLng);
        customOverlay.setMap(map);
    });

    // 다각형에 mousemove 이벤트를 등록하고 이벤트가 발생하면 커스텀 오버레이의 위치를 변경합니다
    kakao.maps.event.addListener(polygon, 'mousemove', function (mouseEvent) {
        customOverlay.setPosition(mouseEvent.latLng);
    });

    // 다각형에 mouseout 이벤트를 등록하고 이벤트가 발생하면 폴리곤의 채움색을 원래색으로 변경합니다
    // 커스텀 오버레이를 지도에서 제거합니다
    kakao.maps.event.addListener(polygon, 'mouseout', function () {
        polygon.setOptions({ fillColor: '#fff' });
        customOverlay.setMap(null);
    });
    //$.ajax({
    //    type: "POST",
    //    url: "WebService.asmx/GetSidoList",
    //    data: "{code : '" + area.name + "',sDate : '2019-09',eDate : '2020-04'}",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        // 다각형을 생성합니다
    //        var polygon = new kakao.maps.Polygon({
    //            map: map, // 다각형을 표시할 지도 객체
    //            path: area.path,
    //            strokeWeight: 2,
    //            strokeColor: '#004c80',
    //            strokeOpacity: 0.8,
    //            fillColor: '#fff',
    //            fillOpacity: 0.3
    //        });
    //        // 다각형에 mouseover 이벤트를 등록하고 이벤트가 발생하면 폴리곤의 채움색을 변경합니다
    //        // 지역명을 표시하는 커스텀오버레이를 지도위에 표시합니다
    //        kakao.maps.event.addListener(polygon, 'mouseover', function (mouseEvent) {
    //            var content = "<div class='area' style='width: 100px; height: 120px;' align='center'>" + area.name
    //            content += "</br> 건수 : " + data.d.cnt + "";
    //            if (data.d.top1 != "") {
    //                content += "</br> TOP 3";
    //                content += "</br> 1. " + data.d.top1 + "";
    //            }
    //            if (data.d.top2 != "") {
    //                content += "</br> 2. " + data.d.top2 + "";
    //            }
    //            if (data.d.top3 != "") {
    //                content += "</br> 3. " + data.d.top3 + "";
    //            }
    //            content += "</div>";

    //            polygon.setOptions({ fillColor: '#09f' });
    //            customOverlay.setContent(content);
    //            customOverlay.setPosition(mouseEvent.latLng);
    //            customOverlay.setMap(map);
    //        });

    //        // 다각형에 mousemove 이벤트를 등록하고 이벤트가 발생하면 커스텀 오버레이의 위치를 변경합니다
    //        kakao.maps.event.addListener(polygon, 'mousemove', function (mouseEvent) {
    //            customOverlay.setPosition(mouseEvent.latLng);
    //        });

    //        // 다각형에 mouseout 이벤트를 등록하고 이벤트가 발생하면 폴리곤의 채움색을 원래색으로 변경합니다
    //        // 커스텀 오버레이를 지도에서 제거합니다
    //        kakao.maps.event.addListener(polygon, 'mouseout', function () {
    //            polygon.setOptions({ fillColor: '#fff' });
    //            customOverlay.setMap(null);
    //        });

    //        // 다각형에 click 이벤트를 등록하고 이벤트가 발생하면 다각형의 이름과 면적을 인포윈도우에 표시합니다
    //        kakao.maps.event.addListener(polygon, 'click', function (mouseEvent) {

    //            removeAllChildNods(document.getElementById('map'));

    //            var mapContainer = document.getElementById('map'), // 지도를 표시할 div
    //                mapOption = {
    //                    center: new kakao.maps.LatLng(36.054837080789724, 127.64352756503625), // 지도의 중심좌표
    //                    level: 13 // 지도의 확대 레벨
    //                };

    //            var map = new kakao.maps.Map(mapContainer, mapOption),
    //                customOverlay = new kakao.maps.CustomOverlay({}),
    //                infowindow = new kakao.maps.InfoWindow({ removable: true });

    //            if (area.name == "서울특별시") {
    //                var moveLatLon = new kakao.maps.LatLng(37.537570478275406, 126.99239656623806);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 서울특별시.length; i < len; i++) {
    //                    sigunguArea(서울특별시[i], map)
    //                }
    //            }
    //            if (area.name == "부산광역시") {
    //                var moveLatLon = new kakao.maps.LatLng(35.19026791438057, 129.04192044179922);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 부산광역시.length; i < len; i++) {
    //                    sigunguArea(부산광역시[i], map)
    //                }
    //            }
    //            if (area.name == "대구광역시") {
    //                var moveLatLon = new kakao.maps.LatLng(35.81386239704274, 128.57063570489186);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 대구광역시.length; i < len; i++) {
    //                    sigunguArea(대구광역시[i], map)
    //                }
    //            }
    //            if (area.name == "인천광역시") {
    //                var moveLatLon = new kakao.maps.LatLng(37.48944296570878, 126.55367665727358);
    //                map.setLevel(10);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 인천광역시.length; i < len; i++) {
    //                    sigunguArea(인천광역시[i], map)
    //                }
    //            }
    //            if (area.name == "광주광역시") {
    //                var moveLatLon = new kakao.maps.LatLng(35.1447466964196, 126.8093214318224);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 광주광역시.length; i < len; i++) {
    //                    sigunguArea(광주광역시[i], map)
    //                }
    //            }
    //            if (area.name == "대전광역시") {
    //                var moveLatLon = new kakao.maps.LatLng(36.32442216387775, 127.39058654893643);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 대전광역시.length; i < len; i++) {
    //                    sigunguArea(대전광역시[i], map)
    //                }
    //            }
    //            if (area.name == "울산광역시") {
    //                var moveLatLon = new kakao.maps.LatLng(35.52049300036615, 129.23436600472218);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 울산광역시.length; i < len; i++) {
    //                    sigunguArea(울산광역시[i], map)
    //                }
    //            }
    //            if (area.name == "세종특별자치시") {
    //                var moveLatLon = new kakao.maps.LatLng(36.556083031767066, 127.24555849935012);
    //                map.setLevel(9);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 세종특별자치시.length; i < len; i++) {
    //                    sigunguArea(세종특별자치시[i], map)
    //                }
    //            }
    //            if (area.name == "경기도") {
    //                var moveLatLon = new kakao.maps.LatLng(37.50008570155095, 126.97213468572798);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 경기도.length; i < len; i++) {
    //                    sigunguArea(경기도[i], map)
    //                }
    //            }
    //            if (area.name == "강원도") {
    //                var moveLatLon = new kakao.maps.LatLng(37.78636226841189, 128.2521964134432);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 강원도.length; i < len; i++) {
    //                    sigunguArea(강원도[i], map)
    //                }
    //            }
    //            if (area.name == "충청북도") {
    //                var moveLatLon = new kakao.maps.LatLng(36.653530943064375, 127.69071544887379);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 충청북도.length; i < len; i++) {
    //                    sigunguArea(충청북도[i], map)
    //                }
    //            }
    //            if (area.name == "충청남도") {
    //                var moveLatLon = new kakao.maps.LatLng(36.52723528336985, 126.74917317447809);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 충청남도.length; i < len; i++) {
    //                    sigunguArea(충청남도[i], map)
    //                }
    //            }
    //            if (area.name == "전라북도") {
    //                var moveLatLon = new kakao.maps.LatLng(35.66952417480411, 127.05902469884327);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 전라북도.length; i < len; i++) {
    //                    sigunguArea(전라북도[i], map)
    //                }
    //            }
    //            if (area.name == "전라남도") {
    //                var moveLatLon = new kakao.maps.LatLng(34.850325064169745, 126.86808685444988);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 전라남도.length; i < len; i++) {
    //                    sigunguArea(전라남도[i], map)
    //                }
    //            }
    //            if (area.name == "경상북도") {
    //                var moveLatLon = new kakao.maps.LatLng(36.36533231287503, 128.72817691356124);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 경상북도.length; i < len; i++) {
    //                    sigunguArea(경상북도[i], map)
    //                }
    //            }
    //            if (area.name == "경상남도") {
    //                var moveLatLon = new kakao.maps.LatLng(35.38430763474538, 128.20834403938085);
    //                map.setLevel(11);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 경상남도.length; i < len; i++) {
    //                    sigunguArea(경상남도[i], map)
    //                }
    //            }
    //            if (area.name == "제주특별자치도") {
    //                var moveLatLon = new kakao.maps.LatLng(33.338740884846494, 126.54773011699562);
    //                map.setLevel(10);
    //                map.setCenter(moveLatLon);

    //                for (var i = 0, len = 제주특별자치도.length; i < len; i++) {
    //                    sigunguArea(제주특별자치도[i], map)
    //                }
    //            }
    //        });
    //    }
    //});
}