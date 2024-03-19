document.addEventListener("DOMContentLoaded", function () {
  const buscarModalInput = document.getElementById("inputContrato");

  buscarModalInput.addEventListener("keyup", function () {
    const searchTerm = buscarModalInput.value;

    fetch(`/Contratos/FiltrarContratos?searchTerm=${searchTerm}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((contratos) => {
        console.log(contratos);
        const contratosList = document.getElementById("ajaxContentContrato");
        contratosList.innerHTML = "";

        contratos.forEach(function (contrato) {
          const inputGroup = document.createElement("div");
          inputGroup.classList.add("d-flex", "align-items-center", "mb-3");

          const inputGroupPrepend = document.createElement("div");
          inputGroupPrepend.classList.add("input-group-prepend", "d-flex");
          const labelGroup = document.createElement("div");
          labelGroup.classList.add("d-flex", "flex-column","w-100");
          const input = document.createElement("input");
          input.type = "radio";
          input.name = "contratoRadio";
          input.value = contrato.id;
          input.classList.add("form-check-input", "me-3");

          const label = document.createElement("label");
          const label2 = document.createElement("label");
          const label3 = document.createElement("label");
          const label4 = document.createElement("label");

          label.classList.add("form-check-label", "input-group-text", "input-top");
          label.textContent = "Contrato N°: " + contrato.id;
          label2.classList.add("form-check-label", "input-group-text", "input-mid");
          label2.textContent =
            "Inquilino: " +
            contrato.inquilino.nombre +
            " " +
            contrato.inquilino.apellido +
            " - DNI: " +
            contrato.inquilino.dni;
          label3.classList.add("form-check-label", "input-group-text", "input-mid");
          label3.textContent = "Monto Mensual : $" + contrato.montoMensual;
          label4.classList.add("form-check-label", "input-group-text", "input-bot");
          label4.textContent =
            "Inmueble: " + contrato.inmueble.tipo + " - " + contrato.inmueble.direccion;

          inputGroupPrepend.appendChild(input);
          inputGroup.appendChild(inputGroupPrepend);
          labelGroup.append(label, label2, label3, label4);
          inputGroup.append(labelGroup);

          contratosList.appendChild(inputGroup);

          input.addEventListener("change", function () {
            if (input.checked) {
              console.log(contrato);
              console.log(input.value);
              document.getElementById("inmuebleDireccion").textContent =
                contrato.inmueble.direccion;
              document.getElementById("inmuebleUso").textContent = contrato.inmueble.uso;
              document.getElementById("inmuebleTipo").textContent =
                contrato.inmueble.tipo;
              document.getElementById("InquilinoNombre").textContent =
                contrato.inquilino.nombre;
              document.getElementById("InquilinoApellido").textContent =
                contrato.inquilino.apellido;
              document.getElementById("InquilinoDni").textContent =
                contrato.inquilino.dni;
              document.getElementById("importeInput").value = contrato.montoMensual;
              document.getElementById("idContrato").value = contrato.id;
              const fechaInicio = new Date(contrato.fechaInicio);


              const year = fechaInicio.getFullYear();
              const month = fechaInicio.getMonth() + 1; 
              const day = fechaInicio.getDate();


              const formattedMonth = month < 10 ? "0" + month : month;
              const formattedDay = day < 10 ? "0" + day : day;

             
              const formattedDate = year + "-" + formattedMonth + "-" + formattedDay;
              
              document.getElementById("fechaPagoInput").min = formattedDate;
              console.log(document.getElementById("fechaPagoInput").min);
            }
          });
        });
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  });
});
