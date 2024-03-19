function mostrarModalCrear(modalId, element) {
  fetch(`http://localhost:5065/${element}s/Create`)
    .then(function (response) {
      if (!response.ok) {
        throw new Error("No se recibió respuesta del servidor.");
      }
      return response.text();
    })
    .then(function (data) {
      const tempElement = document.createElement("div");
      tempElement.innerHTML = data;

      const formElement = tempElement.querySelector("form");
      console.log(formElement);

      if (formElement) {
        const modalContent = document.getElementById("modal-content");
        modalContent.innerHTML = "";
        modalContent.classList.add("text-light");
        modalContent.appendChild(formElement);
        const btn = formElement.querySelector("button[type='button']");
        if (btn) {
          btn.classList.add("d-none");
        }
      } else {
        console.error("No se encontró el formulario.");
      }
    })
    .then(function (response) {
      console.log("form"+element);
      const form = document.getElementById("form"+element);
      console.log(element);
      form.action = `/${element}s/Create`;
      console.log(form.action);
      form.addEventListener("submit", function (event) {
        formInmueble.addEventListener("submit", function (event) {
          console.log("ENTRO FORM INMUEBLE");
          event.preventDefault();
        });
        console.log("ENTRO FORM PROPIETARIO");
      });
    })
    .then(function () {
      const modal = new bootstrap.Modal(document.getElementById(modalId));
      modal.show();
    })
    .catch(function (error) {
      console.error("Error en el Fetch:", error);
    });
}

document.querySelectorAll("button[type='button']").forEach(function (button) {
  if (button.id === "Propietario" || button.id === "Inquilino") {
    console.log("ENTRO"+button.id);
    button.addEventListener("click", function (event) {
      const modalId = `modalCrear${event.target.id}`;
      const element = `${event.target.id}`;
      if (event.target.id !== "") {
        mostrarModalCrear(modalId, element);
      }
    });
  }
});

// document.addEventListener("DOMContentLoaded", function () {
//   const modalCrear = new bootstrap.Modal(document.getElementById("modalCrear"));
//   const formInmueble=document.getElementById("formInmueble");
//   modalCrear._element.addEventListener("show.bs.modal", function (event) {
//     fetch("http://localhost:5065/Propietarios/Create")
//       .then(function (response) {
//         if (!response.ok) {
//           throw new Error("No se recibio respuesta del servidor.");
//         }
//         return response.text();
//       })
//       .then(function (data) {
//         const tempElement = document.createElement("div");
//         tempElement.innerHTML = data;

//         const formElement = tempElement.querySelector("form");

//         if (formElement) {
//           const modalContent = document.getElementById("modal-content");
//           modalContent.innerHTML = "";
//           modalContent.appendChild(formElement);

//           const btn = formElement.querySelector("button[type='button']");
//           if (btn) {
//             btn.classList.add("d-none");
//           }
//         } else {
//           console.error("no se encontro el formulario.");
//         }
//       })
//       .then(function (response) {
//         const form = document.getElementById("form");
//         form.action="/Propietarios/Create";
//         console.log(form.innerHTML);
//         form.addEventListener("submit", function (event) {
//             formInmueble.addEventListener("submit", function (event) {
//                 console.log("ENTRO FORM INMUEBLE");
//               event.preventDefault();
//             })
//           console.log("ENTRO FORM PROPIETARIO");
//         });
//       })
//       .catch(function (error) {
//         console.error("Error en el Fetch:", error);
//       });
//   });
// });
