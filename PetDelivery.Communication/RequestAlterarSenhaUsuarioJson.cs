﻿namespace PetDelivery.Communication;
public class RequestAlterarSenhaUsuarioJson
{
	public string Senha { get; set; } = string.Empty;
	public string NovaSenha { get; set; } = string.Empty;
}
