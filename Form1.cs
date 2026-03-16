using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace Mapa_Mundi
{
    public partial class Form1 : Form
    {
        // Usamos o "?" para indicar que ele pode começar nulo (resolve o warning CS8618)
        private WebView2? webView;

        public Form1()
        {
            // Configuramos o formulário diretamente aqui, já que não temos o Designer.cs
            this.Text = "Mapa Mundi Detalhado";
            this.Size = new System.Drawing.Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            InicializarNavegador();
        }

        private async void InicializarNavegador()
        {
            webView = new WebView2 { Dock = DockStyle.Fill };
            this.Controls.Add(webView);

            try 
            {
                // Aguarda a inicialização do motor Chromium
                await webView.EnsureCoreWebView2Async();

                // HTML com Leaflet - Sem bloqueios e com zoom detalhado
                string htmlMapa = @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8' />
                        <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
                        <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
                        <style>
                            #map { height: 100vh; width: 100%; margin: 0; padding: 0; }
                            body { margin: 0; background-color: #f0f0f0; }
                        </style>
                    </head>
                    <body>
                        <div id='map'></div>
                        <script>
                            // Inicializa o mapa
                            var map = L.map('map').setView([0, 0], 2);

                            // Usando Esri: Mais detalhado, profissional e sem erro 403/Referer
                            L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
                                attribution: 'Tiles &copy; Esri &mdash; Source: Esri, DeLorme, NAVTEQ, USGS, Intermap, iPC, NRCAN, Esri Japan, METI, Esri China (Hong Kong), Esri Korea, Esri (Thailand), MapmyIndia, OpenStreetMap contributors, and the GIS Community'
                            }).addTo(map);
                        </script>
                    </body>
                    </html>";

                webView.NavigateToString(htmlMapa);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o motor do mapa: " + ex.Message);
            }
        }
    }
}