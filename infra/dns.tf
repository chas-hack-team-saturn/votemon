resource "cloudflare_record" "app_dns" {
  zone_id = var.cloudflare_zone_id
  name    = var.domain_name
  content = aws_instance.app_server.public_ip
  type    = "A"
  proxied = true
  comment = "Managed by OpenTofu - Votemon"
}
