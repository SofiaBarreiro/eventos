///*const { forEach } = require("../../fontawesome/js/v4-shims");


function focusById(elementId) {
    var element = document.getElementById(elementId);
    if (element) {
        element.focus();
    }
}
function inhabilitarDNIConf(dni) {

    var inputDNI = document.getElementById("inputDNIConf");
    inputDNI.value = dni;
    inputDNI.disabled = "disabled";

}
function traerModalScanner() {

    var rowModal = document.getElementById("Modalscanner");

    var div = document.getElementById('appendModal');

    div.innerHTML = rowModal.outerHTML;
       
}
function printDiv() {
    var divContents = document.getElementById("PrintDiv").innerHTML;
    var a = window.open('', '_self', 'height=500, width=500');
    a.document.write('<html>');
    a.document.write('<style>table,th,td {border: 1px solid black;border-collapse: collapse;padding: 5px;}tr:nth-child(odd) {background-color: #8F9AA5;}</style >')
    a.document.write('<body> <br>');
    a.document.write(divContents);
    a.document.write('</body></html>');
    a.document.close();
    a.print();
}

function transformarEnPassword() {

    var password = document.getElementById("inputDNIConf");
    const type = password.getAttribute('type') === 'text' ? 'password' : 'text';
    password.setAttribute('type', type);

}
function inhabilitarDNI(dni) {

    var inputDNI = document.getElementById("inputDNIConf");
    inputDNI.value = dni;
    inputDNI.disabled = "disabled";

}
function inhabilitarNombre(nombre) {

    var input = document.getElementById("inputNombre");
    input.disabled = "disabled";
    input.value = nombre;


}
function inhabilitarApellido(apellido) {

    var input = document.getElementById("inputApellido");
    input.disabled = "disabled";
    input.value = apellido;

}
function habilitarDNI(dni) {

    var inputDNI = document.getElementById("inputDNI");
    inputDNI.removeAttribute("disabled");
    inputDNI.value = dni;

}
function focusOutEmail() {

    var inputEmail = document.getElementById("inputNombre");
    if (inputEmail) {
        inputEmail.focus();
        console.log("hizo focus");
    }

}

function habilitarCampo(idCampo) {

    var inputDNI = document.getElementById(idCampo);
    inputDNI.removeAttribute("disabled");
    inputDNI.value = "";

}
function ClickUpload() {
    /// <summary>
    /// Hace click en el boton upload.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("ButtonUpload");
    if (element) {
        element.click();
    }
}
function onScanSuccess(qrCodeMessage) {

    if (qrCodeMessage != null) {

       
        if (qrCodeMessage.includes("https:")) {
            document.getElementById('result').innerHTML = '<span class="result" hidden>' + qrCodeMessage + '</span>';
            document.getElementById('textoResult').textContent = qrCodeMessage;
            window.open(qrCodeMessage, "_blank");
           
        }

    }
}
function onScanError(errorMessage) {
    //handle scan error
    //document.getElementById('result').innerHTML = '<span class="result" >' + errorMessage + '</span>';


}

function initScanner() {
    //var url = "https://intranet.trabajo.gob.ar/SitePages/Inicio.aspx";
    var html5QrcodeScanner = new Html5QrcodeScanner(
        "reader", { fps: 10, qrbox: 250 });

    html5QrcodeScanner.render(onScanSuccess, onScanError);
    //document.getElementById('result').innerHTML = '<span class="result" >' + url + '</span>';
 
    /* return document.getElementsByClassName('result')[0].innerHTML;*/
    return "resultado";

}





function AbrirModal() {
    /// <summary>
    /// Hace click en el boton upload.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("botonModalConfirmar");
    if (element) {
        element.click();
    }
}
function AbrirModalEscanear() {
    /// <summary>
    /// Hace click en el boton upload.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("botonModalEscanear");
    if (element) {
        element.click();
    }
}

function AbrirModalInscribir() {
    /// <summary>
    /// Hace click en el boton upload.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("botonModalInscribir");
    if (element) {
        element.click();
    }
}

function AbrirModalEnviarInscripcion() {
    /// <summary>
    /// Hace click en el boton upload.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("botonModalEnviarInscripcion");
    if (element) {
        element.click();
    }
}
function AbrirModalDNI() {
    /// <summary>
    /// Hace click en el boton upload.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("botonModalDNI");
    if (element) {
        element.click();
    }
}
function cambiarCantidadCaracteres() {

    var input = document.getElementById("inputDescripcion");
    var element = document.getElementById("labelDescripcion");


    input.addEventListener('keyup', (event) => {
        // Check for allowed keys on keydown with Code
        if (event.keyCode === 8 || event.keyCode === 46) {

        }


        var total = input.value.length;
        var resta = 198 - total;
        var mensajeDescripcion;


        if (resta < 0) {

            mensajeDescripcion = "Se ha excedido por: " + Math.abs(resta) + " caracteres. Por favor corrija el campo";
            element.textContent = mensajeDescripcion;
        }
        else {

            mensajeDescripcion = "Caracteres disponibles: " + resta;

        }

        element.textContent = mensajeDescripcion;

    });


}


function agregarEnter() {

    var input = document.getElementById("inputBusqueda");
    var element = document.getElementById("botonBusqueda");


    input.addEventListener('keyup', (event) => {
        

        if (event.keyCode === 13) {
            element.click();
        }


    });


}

function cambiarCantidadTitulo() {

    var input = document.getElementById("txtNombreEvento");
    var element = document.getElementById("labelTitulo");
    element.classList.add("text-danger")

    input.addEventListener('keyup', (event) => {
        // Check for allowed keys on keydown with Code
        if (event.keyCode === 8 || event.keyCode === 46) {

            console.log("presione borrar");

        }


        var total = input.value.length;
        var resta = 148 - total;
        var mensajeDescripcion;


        if (resta < 0) {

            mensajeDescripcion = "Se ha excedido por: " + Math.abs(resta) + " caracteres. Por favor corrija el campo";
            element.textContent = mensajeDescripcion;

        }
        else {

            mensajeDescripcion = "";

        }

        element.textContent = mensajeDescripcion;

    });


}

function agregarComportamientoCuerpoModal() {

    var input = document.getElementById("inputEmail");
    var body = document.getElementById("bodyInscripcion");
    var boton = document.getElementById("botonGuardarInscripcion");


    boton.hidden = true;

    input.addEventListener('keyup', (event) => {
        // Check for allowed keys on keydown with Code
        if (event.keyCode === 8 || event.keyCode === 46) {

        }
        if (input.value == "") {
            bodyInscripcion.hidden = true;
            boton.hidden = true;
            

        } else {

            if (input.value.match(
                /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            )) {
                bodyInscripcion.hidden = false;
                boton.hidden = false;
            } else {

                bodyInscripcion.hidden = true;

            }
        }

        return true;
    });

    return false;
}
function bloquearInscripcion() {


    var boton = document.getElementById("botonGuardarInscripcion");
    boton.hidden = true;

}

function ocultarBodyInscribir() {

    var bodyInscripcion = document.getElementById("bodyInscripcion");
    bodyInscripcion.hidden = true;

}




function SetTime() {


    /// <summary>
    /// atualiza el sitio pasados 10 minutos
    /// </summary>
    /// <returns></returns>
    setTimeout(function () { window.location.replace(window.origin) }, 10000);


}

function checkBrowser() {


    /// <summary>
    /// si abre el sitio en IE lo oculta.
    /// </summary>
    /// <returns></returns>
    var formulario = document.getElementById("formularioEvento");


    var isIE = false || !!document.documentMode;


    if (isIE == false) {

        formulario.hidden = false;

    }
}







function ingresarFormato() {

    /// <summary>
    /// si los checks de formato estan chequeados devuelve el nombre
    /// </summary>
    /// <returns>Formato del evento</returns>
    var element = document.getElementById("presencial");
    var formato = "";

    var elementE = document.getElementById("streaming");

    var elementV = document.getElementById("virtualC");

    var inputFormato = document.getElementById("inputFormato");

    if (element.checked == true) {

        formato = "Presencial"
    }

    if (elementE.checked == true) {

        formato = "Streaming";
    }
    if (elementV.checked == true) {

        formato = "Virtual";
    }

    return formato;
}


function editFormFormato(formato) {

    /// <summary>
    /// si alguno de los checks es editado saca el check de los otros
    /// </summary>
    /// <param name="formato">id del elemento chequeado</param>
    /// <returns></returns>
    var element = document.getElementById("presencial");

    var elementE = document.getElementById("streaming");

    var elementV = document.getElementById("virtualC");



    if (formato == "Presencial") {
        element.checked = true;
        checkOutPresencial();
    }

    if (formato == "Streaming") {

        elementE.checked = true;
        checkOutStreaming();

    }
    if (formato == "Virtual") {
        elementV.checked = true;

        checkOutVirtualC();
    }




}




function checkOutPresencial() {

    /// <summary>
    /// Segun el chek seleccionado oculta o muestra los campos requeridos.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("presencial");

    var elementE = document.getElementById("streaming");

    var elementV = document.getElementById("virtualC");

    var divPresencial = document.getElementById("divPresencial");

    var divStreaming = document.getElementById("divStreaming");

    var divVirtualOStreaming = document.getElementById("divVirtualOStreaming");

    var divVirtual = document.getElementById("divVirtual");





    if (element.checked == true) {

        divPresencial.hidden = false;
        divStreaming.hidden = true;
        divVirtualOStreaming.hidden = true;
        divVirtual.hidden = true;

        document.getElementById("inputVirtual").value = 0;

        document.getElementById("inputVirtualOStreaming").value = "";

        document.getElementById("inputStreaming1").value = "";
        document.getElementById("inputStreaming2").value = "";



        elementE.checked = false;
        elementV.checked = false;
    } else {

        divPresencial.hidden = true;


        document.getElementById("inputPresencial").value = 0;

    }




}

function checkOutStreaming() {
    /// <summary>
    /// Segun el chek seleccionado oculta o muestra los campos requeridos.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("presencial");

    var elementE = document.getElementById("streaming");

    var elementV = document.getElementById("virtualC");
    var divPresencial = document.getElementById("divPresencial");

    var divStreaming = document.getElementById("divStreaming");

    var divVirtualOStreaming = document.getElementById("divVirtualOStreaming");

    var divVirtual = document.getElementById("divVirtual");


    if (elementE.checked == true) {

        element.checked = false;
        elementV.checked = false;


        divPresencial.hidden = true;
        divStreaming.hidden = false;
        divVirtualOStreaming.hidden = false;
        divVirtual.hidden = true;

        document.getElementById("inputPresencial").value = 0;
        document.getElementById("inputVirtual").value = 0;




    } else {


        divStreaming.hidden = true;
        divVirtualOStreaming.hidden = true;

        document.getElementById("inputVirtualOStreaming").value = "";

        document.getElementById("inputStreaming1").value = "";
        document.getElementById("inputStreaming2").value = "";


    }


}
function traerDatoInput() {


    return document.getElementById("inputEsExtranjero").value;

}

function traerDatoInputC() {


    return document.getElementById("inputEsExtranjeroC").value;

}

function checkEsExtranjero() {

    var element = document.getElementById("esExtranjero");

    element.addEventListener('click', (event) => {

        if (element.checked == true) {

            document.getElementById("inputEsExtranjero").value = "1";
            NombreCampo = "Pasaporte *";
            smallMensaje = "Ingrese su número de pasaporte";

            document.getElementById("NombreCampo").innerText = NombreCampo;
            document.getElementById("smallMensaje").innerText = smallMensaje;

        } else {

            document.getElementById("inputEsExtranjero").value = "0";
            NombreCampo = "DNI *";
            smallMensaje = "Si es empleado del MTEySS ingrese su correo institucional y CUIL";

            document.getElementById("NombreCampo").innerText = NombreCampo;
            document.getElementById("smallMensaje").innerText = smallMensaje;
        }
    });



}

function checkEsExtranjeroC() {

    var element = document.getElementById("esExtranjeroC");

    element.addEventListener('click', (event) => {

        if (element.checked == true) {

            document.getElementById("inputEsExtranjeroC").value = "1";
            NombreCampo = "Pasaporte *";
            smallMensaje = "Ingrese su número de pasaporte";

            document.getElementById("NombreCampoC").innerText = NombreCampo;
            document.getElementById("smallMensajeC").innerText = smallMensaje;

        } else {

            document.getElementById("inputEsExtranjeroC").value = "0";
            NombreCampo = "DNI *";
            smallMensaje = "Si es empleado del MTEySS ingrese su correo institucional y CUIL";

            document.getElementById("NombreCampoC").innerText = NombreCampo;
            document.getElementById("smallMensajeC").innerText = smallMensaje;
        }
    });



}

function checkOutVirtualC() {

    /// <summary>
    /// Segun el chek seleccionado oculta o muestra los campos requeridos.
    /// </summary>
    /// <returns></returns>
    var divPresencial = document.getElementById("divPresencial");


    var element = document.getElementById("presencial");

    var elementE = document.getElementById("streaming");

    var elementV = document.getElementById("virtualC");


    var divPresencial = document.getElementById("divPresencial");

    var divStreaming = document.getElementById("divStreaming");

    var divVirtualOStreaming = document.getElementById("divVirtualOStreaming");

    var divVirtual = document.getElementById("divVirtual");


    if (elementV.checked == true) {

        elementE.checked = false;
        element.checked = false;
        document.getElementById("inputPresencial").value = 0;
        document.getElementById("inputStreaming1").value = "";
        document.getElementById("inputStreaming2").value = "";

        divPresencial.hidden = true;
        divStreaming.hidden = true;
        divVirtualOStreaming.hidden = false;
        divVirtual.hidden = false;
    } else {

        document.getElementById("inputVirtual").value = 0;
        document.getElementById("inputVirtualOStreaming").value = "";

        divVirtualOStreaming.hidden = true;
        divVirtual.hidden = true;
    }


}


function agregarPorcentajeABarra(porcentaje) {
    /// <summary>
    /// Agregars the porcentaje a barra.
    /// </summary>
    /// <param name="porcentaje">el porcentaje.</param>
    /// <returns></returns>
    var element = document.getElementById("barraProgreso");

    if (porcentaje > 10) {

        element.hidden = false;
    }

    document.getElementsByClassName('progress-bar').item(0).setAttribute('aria-valuenow', porcentaje);
    document.getElementsByClassName('progress-bar').item(0).setAttribute('style', 'width:' + Number(porcentaje) + '%');
    document.getElementsByClassName('progress-bar').item(0).innerText = porcentaje + '%';


    if (porcentaje == 100) {

        element.hidden = true;
    }

}



function changeInputUpload() {

    /// <summary>
    /// hace click en el boton ButtonEnviarFile.
    /// </summary>
    /// <returns></returns>
    var element = document.getElementById("ButtonEnviarFile");


    if (element) {
        element.click();
    }

}

function alertQuinientosInvitados(labelAlert) {

    var element = document.getElementById(labelAlert);


    element.textContent = "Se encontraron más de 500 invitados, aguarde mientras se cargan.";

}
function alertSeCargaronInvitados(texto) {

    var element = document.getElementById("labelAlert");


    element.textContent = texto + " Aguarde mientras se carga el resto";

}
function alertSeCargaronInvitadosConTexto(texto) {

    var element = document.getElementById("labelAlert");


    element.textContent = texto;

}
function CrearQr(url) {

    if (document.getElementById("QRCodeStr") != null) {

        ArmarQr(url);

    } else {

        setTimeout(function () {

            if (document.getElementById("QRCodeStr") != null) {
                ArmarQr(url);
                //document.getElementById("QRCodeStr").display = "block";

            }


        }, 3000);

    }
    setTimeout(function () {

        if (document.getElementById("QRCodeStr") != null) {

        //    document.getElementById("QRCodeStr").display = "block";
        }


    }, 3000);

}
function ArmarQr(url) {

    var qrcode = new QRCode(document.getElementById("QRCodeStr"), {
        text: url,
        width: 250,
        height: 250,
        colorDark: "#000000",
        colorLight: "#ffffff",
      /*  display:"none",*/

        correctLevel: QRCode.CorrectLevel.H
    });

}




function borrarChecks() {

    var element = document.getElementsByClassName("checkbox");

    var rutas = document.getElementById("rutas");

    var listado = rutas.value;

    if (element) {

        listado = "";
        for (var i = 0; i < element.length; i++) {

            if (element[i].checked == false) {

                var elementNOquitar = element[i].id;

                if (listado == "") {

                    listado = elementNOquitar;
                } else {

                    listado = listado + ", " + elementNOquitar;

                }


            }


        }

    }

    document.getElementById("rutas").value = listado;
}
