using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace droneWEB.WebUI.Helpers
{
    public static class CaptchaHelper
    {
        public static IHtmlContent RenderCaptcha(this IHtmlHelper htmlHelper, string inputId = "captchaInput")
        {
            var captchaId = "captchaCanvas";
            var script = $@"
                <canvas id='{captchaId}' width='150' height='50' style='border:1px solid #00d4ff; margin-bottom:10px;'></canvas>
               <input type='text' id='{inputId}' name='{inputId}' 
       placeholder='kodu girin' 
       class='form-control mb-2' 
       style='width:150px; height:35px; font-size:14px; text-align:center; display:block; margin:0 auto;' />


                <button type='button' id='refreshCaptcha' class='btn btn-sm btn-outline-light mb-2'>Yenile</button>

                <script>
                    function generateCaptcha() {{
                        var canvas = document.getElementById('{captchaId}');
                        var ctx = canvas.getContext('2d');
                        ctx.clearRect(0,0,canvas.width,canvas.height);

                        var chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
                        var captchaText = '';
                        for(var i=0;i<6;i++) {{
                            captchaText += chars.charAt(Math.floor(Math.random() * chars.length));
                        }}

                        // arka plan
                        ctx.fillStyle = '#25254A';
                        ctx.fillRect(0,0,canvas.width,canvas.height);

                        // yazı
                        ctx.font = '25px Arial';
                        ctx.fillStyle = '#00d4ff';
                        ctx.textBaseline = 'middle';
                        ctx.textAlign = 'center';
                        ctx.fillText(captchaText, canvas.width/2, canvas.height/2);

                        // rastgele çizgiler
                        for(var i=0;i<5;i++){{
                            ctx.strokeStyle = '#00d4ff';
                            ctx.beginPath();
                            ctx.moveTo(Math.random()*canvas.width, Math.random()*canvas.height);
                            ctx.lineTo(Math.random()*canvas.width, Math.random()*canvas.height);
                            ctx.stroke();
                        }}

                        // rastgele noktalar
                        for(var i=0;i<30;i++){{
                            ctx.fillStyle = '#00d4ff';
                            ctx.beginPath();
                            ctx.arc(Math.random()*canvas.width, Math.random()*canvas.height, 1, 0, 2*Math.PI);
                            ctx.fill();
                        }}

                        sessionStorage.setItem('captchaValue', captchaText);
                    }}

                    generateCaptcha();

                    document.getElementById('refreshCaptcha').addEventListener('click', generateCaptcha);

                    document.querySelector('form').addEventListener('submit', function(e){{
                        var userInput = document.getElementById('{inputId}').value.toUpperCase();
                        var captchaVal = sessionStorage.getItem('captchaValue');
                        if(userInput !== captchaVal){{
                            e.preventDefault();
                            alert('Captcha yanlış, lütfen tekrar deneyin.');
                        }}
                    }});
                </script>
            ";
            return new HtmlString(script);
        }
    }
}
