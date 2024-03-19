document.addEventListener("DOMContentLoaded", function () {
    const buscarModalInput = document.getElementById("buscarModal");
  
    buscarModalInput.addEventListener("keyup", function () {
      const searchTerm = buscarModalInput.value;
  
      fetch(`/Inquilinos/FiltrarInquilinos?searchTerm=${searchTerm}`)
        .then((response) => {
          if (!response.ok) {
            throw new Error("Network response was not ok");
          }
          return response.json();
        })
        .then((inquilinos) => {
          console.log(inquilinos);
          const inquilinosList = document.getElementById("ajaxContentInquilino");
          inquilinosList.innerHTML = ""; // Limpia la lista antes de agregar inquilinos
  
          inquilinos.forEach(function (inquilino) {
            const inputGroup = document.createElement("div");
            inputGroup.classList.add("d-flex", "align-items-center", "mb-3");
  
            const inputGroupPrepend = document.createElement("div");
            inputGroupPrepend.classList.add("input-group-prepend");
  
            const input = document.createElement("input");
            input.type = "radio";
            input.name = "inquilinoRadio";
            input.value = inquilino.id;
            input.classList.add("form-check-input", "me-3");
  
            const label = document.createElement("label");
            label.classList.add("form-check-label", "input-group-text","w-100");
            label.textContent =
            inquilino.nombre +
              " " +
              inquilino.apellido +
              " - DNI: " +
              inquilino.dni;
  
            inputGroupPrepend.appendChild(input);
            inputGroup.appendChild(inputGroupPrepend);
            inputGroup.appendChild(label);
  
            inquilinosList.appendChild(inputGroup);
  
            input.addEventListener("change", function () {
                
              if (input.checked) {
                console.log(inquilino);
                const idInquilino = document.getElementById("idInquilino");
                console.log(input.value);
                cardProp.classList.remove("d-none");
                document.getElementById("inquilinoNombre").textContent =
                inquilino.nombre;
                document.getElementById("inquilinoApellido").textContent =
                inquilino.apellido;
                document.getElementById("inquilinoCorreo").textContent =
                inquilino.correo;
                document.getElementById("inquilinoTelefono").textContent =
                inquilino.telefono;
                document.getElementById("busquedaDni").value = inquilino.dni;
                idInquilino.value = inquilino.id;
                btnRegistrar.disabled = false;
                // document.getElementById("datosInmueble").classList.remove("d-none");
                document.getElementById("formDates").classList.remove("d-none");
                
              }
            });
          });
        })
        .catch((error) => {
          console.error("Error fetching data:", error);
        });
    });
  });