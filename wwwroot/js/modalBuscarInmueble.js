document.addEventListener("DOMContentLoaded", function () {
  const buscarModalInput = document.getElementById("inputInmueble");
  const fechaInicioInput = document.getElementById("fechaInicio");
  const fechaFinInput = document.getElementById("fechaFin");

  fechaInicioInput.addEventListener("change", (event) => {
    if (fechaInicioInput.value) {
      fechaFinInput.disabled = false;
    } else {
      fechaFinInput.disabled = true;
    }
  });

  fechaFinInput.addEventListener("change", (event) => {
    if (fechaFinInput.value) {
      document.getElementById("datosInmueble").classList.remove("d-none");
    } else {
      document.getElementById("datosInmueble").classList.add("d-none");
    }
  });

  

  buscarModalInput.addEventListener("keyup", function () {

    const searchTerm = buscarModalInput.value;
    const fechaInicio=fechaInicioInput.value;
    const fechaFin=fechaFinInput.value;


    fetch(
      `/Inmuebles/FiltrarInmuebles?searchTerm=${searchTerm}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((inmuebles) => {
        console.log(inmuebles);
        const inmueblesList = document.getElementById("ajaxContentInmueble");
        inmueblesList.innerHTML = "";

        inmuebles.forEach(function (inmueble) {
          console.log(inmueble);
          const inputGroup = document.createElement("div");
          inputGroup.classList.add("d-flex", "align-items-center", "mb-3");

          const inputGroupPrepend = document.createElement("div");
          inputGroupPrepend.classList.add("input-group-prepend", "d-flex");
          const labelGroup = document.createElement("div");
          labelGroup.classList.add("d-flex", "flex-column","w-100");
          const input = document.createElement("input");
          input.type = "radio";
          input.name = "inmuebleRadio";
          input.value = inmueble.id;
          input.classList.add("form-check-input", "me-3");

          const label = document.createElement("label");
          const label2 = document.createElement("label");
          const label3 = document.createElement("label");
          label.classList.add("form-check-label", "input-group-text", "input-top");
          label.textContent =
           
            inmueble.propietario.nombre +
            " " +
            inmueble.propietario.apellido +
            
            inmueble.propietario.dni;
          label2.classList.add("form-check-label", "input-group-text", "input-mid");
          label2.textContent = "Tipo: " + inmueble.tipo;
          label3.classList.add("form-check-label", "input-group-text", "input-bot");
          label3.textContent = "Direccion: " + inmueble.direccion;

          inputGroupPrepend.appendChild(input);
          inputGroup.appendChild(inputGroupPrepend);
          labelGroup.append(label, label2, label3);
          inputGroup.append(labelGroup);

          inmueblesList.appendChild(inputGroup);

          input.addEventListener("change", function () {
            if (input.checked) {
              document.getElementById("inmueblePropietario").textContent =
                inmueble.propietario.apellido + " " + inmueble.propietario.nombre;
              document.getElementById("inmuebleDireccion").textContent =
                inmueble.direccion;
              document.getElementById("inmuebleUso").textContent = inmueble.uso;
              document.getElementById("inmuebleTipo").textContent = inmueble.tipo;
              document.getElementById("inmuebleCantAmbientes").textContent =
                inmueble.cantAmbientes;
              document.getElementById("inmuebleLatitud").textContent = inmueble.latitud;
              document.getElementById("inmuebleLongitud").textContent = inmueble.longitud;
              document.getElementById("inmueblePrecio").textContent =
                "$" + inmueble.precio;
              document.getElementById("idInmueble").value = inmueble.id;
              document.getElementById("busquedaId").value = "Inmueble N°: " + inmueble.id;
              document.getElementById("cardInmueble").classList.remove("d-none");
              document.getElementById("formInputs").classList.remove("d-none");
              document.getElementById("montoMensual").value = inmueble.precio;
            }
          });
        });
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  });
});
