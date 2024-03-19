const fechaInicio = document.getElementById('fechaInicio');
const fechaFin = document.getElementById('fechaFin');
const montoMensual = document.getElementById('montoMensual');
const formContrato = document.getElementById('formContrato');
fechaInicio.addEventListener('change', () => {
    if (fechaInicio.value) {
        fechaFin.disabled = false;
    } else {
        fechaFin.disabled = true;
    }
});

formContrato.addEventListener('change', () => {
    if (fechaInicio.value&&fechaFin.value&&montoMensual.value) {
document.getElementById('btnRegistrar').disabled = false;
    }else{
        document.getElementById('btnRegistrar').disabled = true; 
    }
});
