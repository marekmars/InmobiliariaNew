function confirmarEliminacion(event) {
  event.preventDefault();
  Swal.fire({
    title: "¿Estás seguro de eliminar el registro?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      event.target.submit();
    }
  });
}
function confirmarEliminacionTotal(event) {
  event.preventDefault();
  Swal.fire({
    title: "¿Estás seguro de eliminar el registro, se daran de baja los inmuebles relacionados con este propietario?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      event.target.submit();
    }
  });
}
function confirmarModificacion(event) {
  event.preventDefault();
  Swal.fire({
    title: "¿Estás seguro de modificar este registro?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Sí, modificar",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      event.target.submit();
    }
  });
}
function confirmarCreacion(event) {
  event.preventDefault();
  Swal.fire({
    title: "¿Estás seguro de crear el registro?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Sí, crear",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      event.target.submit();
    }
  });
}
function confirmarPago(event) {
  event.preventDefault();
  Swal.fire({
    title: "¿Estás seguro de realizar el pago?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Sí, realizar",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      event.target.submit();
    }
  });
}
function mostrarAlertaConSelect(event, formId) {
  event.preventDefault();

  const inputOptions = {
    delete: "Eliminar",
    resignar: "Resignar",
  };

  Swal.fire({
    title: "Selecciona una opción",
    input: "radio",
    inputOptions: inputOptions,
    showCancelButton: true,
    customClass: {
      container: "bg-dark",
    },
    inputValidator: (opcion) => {
      if (!opcion) {
        return "Debes elegir una opción.";
      }
    },
  }).then((result) => {
    if (result.isConfirmed) {
      const selectedOption = result.value;
      const formElement = document.getElementById(formId);

      if (selectedOption === "delete") {
        // Acción de eliminar
        console.log(formElement.action);
        confirmarEliminacion(event);

      } else if (selectedOption === "resignar") {
        // Acción de resignar
        const searchParams = new URLSearchParams();
        searchParams.append("resignacion", "true");

       
        formElement.action = formElement.action + "?" + searchParams.toString();
        console.log(formElement.action);
        formElement.submit();
      }
    }
  });
}