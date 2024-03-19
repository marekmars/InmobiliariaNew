const inputFile=document.getElementById("avatar");
inputFile.addEventListener("change",(event)=>
{
    console.log("ENTRO");
    const preview = document.getElementById('avatarImg');
    const fileInput = event.target;

    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();

        reader.onload = function(e) {
            preview.src = e.target.result;
        };

        reader.readAsDataURL(fileInput.files[0]);
    }
});

