
document.addEventListener("DOMContentLoaded", function () {
  const buscarModalInput = document.getElementById("buscarModal");

  buscarModalInput.addEventListener("keyup", function () {
    console.log("SADASDSAD");
    const searchTerm = buscarModalInput.value;

    fetch(`/Propietario/FiltrarPropietarios?searchTerm=${searchTerm}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((propietarios) => {
        console.log(propietarios);
        const propietariosList = document.getElementById("ajaxContent");
        propietariosList.innerHTML = ""; // Limpia la lista antes de agregar propietarios

        propietarios.forEach(function (propietario) {
          const inputGroup = document.createElement("div");
          inputGroup.classList.add("d-flex", "align-items-center", "mb-3");

          const inputGroupPrepend = document.createElement("div");
          inputGroupPrepend.classList.add("input-group-prepend");

          const input = document.createElement("input");
          input.type = "radio";
          input.name = "propietarioRadio";
          input.value = propietario.id;
          input.classList.add("form-check-input", "me-3");

          const label = document.createElement("label");
          label.classList.add("form-check-label", "input-group-text","w-100");
          label.textContent =
            propietario.nombre +
            " " +
            propietario.apellido +
            " - DNI: " +
            propietario.dni;

          inputGroupPrepend.appendChild(input);
          inputGroup.appendChild(inputGroupPrepend);
          inputGroup.appendChild(label);

          propietariosList.appendChild(inputGroup);

          input.addEventListener("change", function () {
            if (input.checked) {
              const idPropietario = document.getElementById("idPropietario");
              console.log(input.value);
              cardProp.classList.remove("d-none");
              document.getElementById("propietarioNombre").textContent =
                propietario.nombre;
              document.getElementById("propietarioApellido").textContent =
                propietario.apellido;
              document.getElementById("propietarioCorreo").textContent =
                propietario.correo;
              document.getElementById("propietarioTelefono").textContent =
                propietario.telefono;
              document.getElementById("busquedaDni").value = propietario.dni;
              idPropietario.value = propietario.id;
              btnRegistrar.disabled = false;
              formInputs.classList.remove("d-none");
            }
          });
        });
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  });
});
