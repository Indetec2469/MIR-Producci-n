// Descripcion: Buscamos el valor que no sea repetir o igual como el GROUP BY
// Entrada: objectArray = los objetos array, property = key o columna o clave
// Salida: los objetos array solo KEY o COLUMNA o CLAVE
functionGroupBy = (objectArray, property) => {
    let result = [];
    objectArray.map(obj => {
        if (!result.some(_obj => _obj == obj[property]))
            result.push(obj[property]);
    });
    return result;
}

function2DecimalsRound = (value) => {
    return parseFloat(parseFloat(value).toFixed(2));
}

functionPercentageV2IsV1 = (value2, value1) => {
    return (100 / value2) * value1;
}

// Convertir la fecha /Date(***)/ a Fecha
functionConvertDateStringToDate = (dateString) => {
    return new Date(parseInt(dateString.substr(6)));
}

// Obtener mes y numero de mes
// Parametro: mes -> string
funcionObtenerMes = (mes) => {
    switch (mes) {
        case 1:
            return 'Enero';
        case 2:
            return 'Febrero';
        case 3:
            return 'Marzo';
        case 4:
            return 'Abril';
        case 5:
            return 'Mayo';
        case 6:
            return 'Junio';
        case 7:
            return 'Julio';
        case 8:
            return 'Agosto';
        case 9:
            return 'Septiembre';
        case 10:
            return 'Octubre';
        case 11:
            return 'Noviembre';
        case 12:
            return 'Diciembre';
    }
}
funcionObtenerNumeroMes = (mes) => {
    switch (mes) {
        case 'Enero':
            return 1;
        case 'Febrero':
            return 2;
        case 'Marzo':
            return 3;
        case 'Abril':
            return 4;
        case 'Mayo':
            return 5;
        case 'Junio':
            return 6;
        case 'Julio':
            return 7;
        case 'Agosto':
            return 8;
        case 'Septiembre':
            return 9;
        case 'Octubre':
            return 10;
        case 'Noviembre':
            return 11;
        case 'Diciembre':
            return 12;
    }
}
funcionObtenerCodigoMes = (mes) => {
    switch (mes) {
        case 'Enero':
            return 'ENE';
        case 'Febrero':
            return 'FEB';
        case 'Marzo':
            return 'MAR';
        case 'Abril':
            return 'ABR';
        case 'Mayo':
            return 'MAY';
        case 'Junio':
            return 'JUN';
        case 'Julio':
            return 'JUL';
        case 'Agosto':
            return 'AGO';
        case 'Septiembre':
            return 'SEP';
        case 'Octubre':
            return 'OCT';
        case 'Noviembre':
            return 'NOV';
        case 'Diciembre':
            return 'DIC';
    }
}